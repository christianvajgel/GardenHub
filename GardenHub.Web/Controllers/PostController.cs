﻿using System;
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
using GardenHub.Web.ViewModel.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [Authorize]
        public async Task<IActionResult> Home()
        {
            try
            {
                var user = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject")).ToString();
                var account = await this.AccountService.FindById(new Guid(user.ToString()));

                var listAllPosts = this.PostServices.GetAll();
                var posts = new List<Post>();

                await foreach (var post in listAllPosts)
                {
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
            catch
            {
                return View("Login");
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
                        (file != null && String.IsNullOrWhiteSpace(description)) ? PostType.Image : PostType.ImageText;

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

                //urlAzure = await this.AzureStorage.SaveToStorage(ms.ToArray(), $"{Guid.NewGuid().ToString().Replace("-", "")}.jpg");
                urlAzure = await this.AzureStorage.SaveToStorage(ms.ToArray(), azureFilename);
            }

            try
            {
                var account = new Account();

                var user = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject"));
                account = await this.AccountService.FindById(new Guid(user.ToString()));

                post.Description = description;
                post.Account = account;
                post.Image = urlAzure;

                post.Comments = null;

                await this.PostServices.SavePost(post);
                return RedirectToAction("Home");
            }
            catch
            {
                return Redirect("/");
            }
        }

        // GET: PostController/Edit/5
        public ActionResult Edit()
        {
            return View(this.PostServices.FindById(new Guid(RouteData.Values["id"].ToString())));
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromForm] IFormFile? file, [FromForm] string description)
        {
            var idFromRoute = new Guid(RouteData.Values["id"].ToString());
            var post = this.PostServices.FindById(new Guid(RouteData.Values["id"].ToString()));

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
                    //
                    this.AzureStorage.DeleteBlob(post.AzureFilename);
                    post.Image = urlAzure;
                    post.AzureFilename = azureFilename;
                }
                post.Description = description;

                await this.PostServices.EditPost(idFromRoute, post);
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
                var account = new Account();
                var idFromRoute = new Guid(RouteData.Values["id"].ToString());

                var user = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject"));

                account = await this.AccountService.FindById(new Guid(user.ToString()));
                //account = await this.AccountService.FindById(new Guid(JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject")).ToString()));

                var post = this.PostServices.FindById(idFromRoute);

                await this.PostServices.DeletePost(idFromRoute, account);
                
                //await this.PostServices.DeletePost(idFromRoute, await this.AccountService.FindById(new Guid(JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject")).ToString())));

                if (post.Type == PostType.Image || post.Type == PostType.ImageText)
                {
                    this.AzureStorage.DeleteBlob(post.AzureFilename);
                }

                return Redirect("/");
            }
            catch
            {
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
