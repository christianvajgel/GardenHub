using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GardenHub.CrossCutting.Storage;
using GardenHub.Domain.Account;
using GardenHub.Domain.Comment;
using GardenHub.Domain.Post;
using GardenHub.Services.Account;
using GardenHub.Services.Comment;
using GardenHub.Services.Post;
using GardenHub.Token.Service;
using GardenHub.Web.ViewModel.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace GardenHub.Web.Controllers
{
    public class PostController : Controller
    {
        private IAccountService AccountService { get; set; }
        private IPostServices PostServices { get; set; }
        private ICommentServices CommentServices { get; set; }

        private AzureStorage AzureStorage { get; set; }

        public PostController(IAccountService accountService, IPostServices postServices, ICommentServices commentServices, AzureStorage azureStorage)
        //public PostController(IPostServices postServices)
        {
            this.AccountService = accountService;

            this.PostServices = postServices;

            this.CommentServices = commentServices;

            this.AzureStorage = azureStorage;

        }

        public Account GetAccount() 
        {
            var client = new RestClient();
            var userId = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserId")).ToString();
            var email = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserEmail")).ToString();
            var password = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserPassword")).ToString();

            var requestAccount = new RestRequest("https://localhost:5003/api/account/" + userId);

            requestAccount.AddHeader("Authorization", "Bearer " + GardenHub.Token.Service.Token.Generate(email, password));

            return client.Get<Account>(requestAccount).Data;
        }

        public Post GetPost(Guid postId) 
        {
            var client = new RestClient();
            var email = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserEmail")).ToString();
            var password = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserPassword")).ToString();

            var requestPost = new RestRequest("https://localhost:5003/api/post/" + postId);

            requestPost.AddHeader("Authorization", "Bearer " + GardenHub.Token.Service.Token.Generate(email, password));

            return client.Get<Post>(requestPost).Data;
        }

        [Authorize]
        public async Task<IActionResult> Home()
        {
            try
            {
                var account = GetAccount();
                var posts = new List<Post>();
                var client = new RestClient();
                var user = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserId")).ToString();
                var email = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserEmail")).ToString();
                var password = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserPassword")).ToString();


                //var account = await this.AccountService.FindById(new Guid(user.ToString()));

                //var requestAccount = new RestRequest("https://localhost:5003/api/account/" + user);
                //requestAccount.AddHeader("Authorization", "Bearer " + GardenHub.Token.Service.Token.Generate(email, password));
                //var accountResponse = client.Get<Account>(requestAccount).Data;

                //var listAllPosts = this.PostServices.GetAll();
                var requestPosts = new RestRequest("https://localhost:5003/api/post/");
                requestPosts.AddHeader("Authorization", "Bearer " + GardenHub.Token.Service.Token.Generate(email, password));
                var postsResponse = client.Get<IEnumerable<Post>>(requestPosts).Data;

                if (postsResponse == null) { RedirectToAction("Home","Post"); }

                //await foreach (var post in postsResponse)
                foreach (var post in postsResponse)
                {
                    post.Account = account;
                    posts.Add(post);
                }

                var homeViewModel = new HomePostViewModel()
                {
                    Posts = posts,
                    Account = account
                };
                //this.PostServices.GetAll();
                //return View(await AccountService.FindById(new Guid(user)));
                return View(homeViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return RedirectToAction("Login","Account");
            }
            //return View();
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(Post post)
        public async Task<ActionResult> Create([FromForm] IFormFile? file, [FromForm] string description)
        {
            var post = new Post();
            string urlAzure = null;

            post.Type = (file == null && !String.IsNullOrWhiteSpace(description)) ? PostType.Text :
                        (file != null &&  String.IsNullOrWhiteSpace(description)) ? PostType.Image : PostType.ImageText;

            if (file != null)
            {
                var ms = new MemoryStream();

                if (file.ContentType.ToLower() != "image/jpg" &&
                    file.ContentType.ToLower() != "image/jpeg" &&
                    file.ContentType.ToLower() != "image/png" &&
                    file.ContentType.ToLower() != "image/bmp")
                {
                    ModelState.AddModelError("Image", "Image deve ser com a extensão .jpg, .png ou bmp");
                    return View();
                }

                using (var fileUpload = file.OpenReadStream())
                {
                    await fileUpload.CopyToAsync(ms);
                    fileUpload.Close();
                }

                var azureFilename = $"{Guid.NewGuid().ToString().Replace("-", "")}.jpg";
                post.AzureFilename = azureFilename;

                urlAzure = await this.AzureStorage.SaveToStorage(ms.ToArray(), azureFilename);
            }

            try
            {
                var client = new RestClient();
                var email = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserEmail")).ToString();
                var password = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserPassword")).ToString();

                //account = await this.AccountService.FindById(new Guid(user.ToString()));
                var account = GetAccount();

                post.Description = description;
                post.Account = account;
                post.Image = urlAzure;
                post.Comments = null;

                //await this.PostServices.SavePost(post);
                var requestPost = new RestRequest("https://localhost:5003/api/post/create");
                requestPost.AddHeader("Authorization", "Bearer " + GardenHub.Token.Service.Token.Generate(email, password));
                var postResponse = await client.PostAsync<Post>(requestPost);

                return RedirectToAction("Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return Redirect("/");
            }
        }

        // GET: PostController/Edit/5
        public ActionResult Edit()
        {
            //return View(this.PostServices.FindById(new Guid(RouteData.Values["id"].ToString())));
            return View(GetPost(new Guid(RouteData.Values["id"].ToString())));
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromForm] IFormFile? file, [FromForm] string description)
        {
            //var idFromRoute = new Guid(RouteData.Values["id"].ToString());
            var email = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserEmail")).ToString();
            var password = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserPassword")).ToString();
            //var post = this.PostServices.FindById(new Guid(RouteData.Values["id"].ToString()));
            var post = GetPost(new Guid(RouteData.Values["id"].ToString()));

            try
            {
                if (file == null && post.Type == PostType.Text)
                {
                    post.Image = null;
                    post.AzureFilename = null;
                }
                else
                {
                    var ms = new MemoryStream();

                    if (file.ContentType.ToLower() != "image/jpg" &&
                        file.ContentType.ToLower() != "image/jpeg" &&
                        file.ContentType.ToLower() != "image/png" &&
                        file.ContentType.ToLower() != "image/bmp")
                    {
                        ModelState.AddModelError("Image", "Image deve ser com a extensão .jpg, .png ou bmp");
                        return View();
                    }

                    using (var fileUpload = file.OpenReadStream())
                    {
                        await fileUpload.CopyToAsync(ms);
                        fileUpload.Close();
                    }

                    var azureFilename = $"{Guid.NewGuid().ToString().Replace("-", "")}.jpg";
                    var urlAzure = await this.AzureStorage.SaveToStorage(ms.ToArray(), azureFilename);
                    
                    this.AzureStorage.DeleteBlob(post.AzureFilename);
                    post.Image = urlAzure;
                    post.AzureFilename = azureFilename;
                }
                post.Description = description;

                //await this.PostServices.EditPost(idFromRoute, post);
                var client = new RestClient();
                var requestAuthor = new RestRequest("https://localhost:5003/api/post/edit/");
                requestAuthor.AddJsonBody(JsonConvert.SerializeObject(new
                {
                    Id = post.Id,
                    Image = post.Image,
                    AzureFilename = post.AzureFilename,
                    Description = post.Description,
                    Type = post.Type
                }));
                requestAuthor.AddHeader("Authorization", "Bearer " + GardenHub.Token.Service.Token.Generate(email, password));

                await client.PutAsync<Post>(requestAuthor);

                return Redirect("/");
            }
            catch
            {
                return Redirect("/");
            }
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("/Post/Home", Name = "Id")]
        //public async Task<ActionResult> Delete([FromRoute] Guid postId)
        public async Task<ActionResult> Delete()
        {
            try
            {
                var account = GetAccount();
                var post = GetPost(new Guid(RouteData.Values["id"].ToString()));
                var email = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserEmail")).ToString();
                var password = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserPassword")).ToString();


                //await this.PostServices.DeletePost(idFromRoute, account);

                //await this.PostServices.DeletePost(idFromRoute, await this.AccountService.FindById(new Guid(JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject")).ToString())));


                var client = new RestClient();
                var requestPost = new RestRequest("https://localhost:5003/api/post/delete/" + post.Id);
                requestPost.AddHeader("Authorization", "Bearer " + GardenHub.Token.Service.Token.Generate(email, password));

                await client.DeleteAsync<Post>(requestPost);

                if (post.Type == PostType.Image || post.Type == PostType.ImageText)
                {
                    this.AzureStorage.DeleteBlob(post.AzureFilename);
                }

                return Redirect("/");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return Redirect("/");
            }
        }

        public async Task<ActionResult> CreateCommentAsync()
        {
            var user = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject")).ToString();
            var idFromRoute = new Guid(RouteData.Values["id"].ToString());
            var post = this.PostServices.FindById(idFromRoute);
            var listAllComment = this.CommentServices.GetAll();
            var comments = new List<Comment>();
            var account = await this.AccountService.FindById(new Guid(user.ToString()));

            if(listAllComment != null)
            {

            await foreach (var comment in listAllComment)
            {
                if(comment.Post.Id == post.Id)
                {
                    comments.Add(comment);
                }
                
            }

            }

            var postViewModel = new PostViewModel()
            {
                Comments = comments,
                Account = account
            };
            return View(postViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateComment(Guid id, Comment comment)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(comment);

                var account = new Account();
                var user = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject"));
                account = await this.AccountService.FindById(new Guid(user.ToString()));

                var post = PostServices.FindById(id);

                comment.AccountOwnerId = account.Id;
                comment.PostedTime = DateTime.UtcNow;
                comment.Post = post;
                await CommentServices.SaveComment(comment);

                return RedirectToAction("CreateComment");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("APP_ERROR", ex.Message);
                return View(comment);
            }
        }

        public ActionResult EditComment()
        {
            var id = new Guid(RouteData.Values["id"].ToString());
            return Redirect("../../Comment/Edit/" + id);
        }

        public ActionResult DeleteComment()
        {
            var id = new Guid(RouteData.Values["id"].ToString());
            return Redirect("../../Comment/Delete/" + id);
        }

        public ActionResult DetailComment()
        {
            return RedirectToAction("Detail", "Comment");
        }

    }
}
