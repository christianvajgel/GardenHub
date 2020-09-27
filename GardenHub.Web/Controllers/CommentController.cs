using System;
using System.Threading.Tasks;
using GardenHub.Domain.Comment;
using GardenHub.Domain.Post;
using GardenHub.Services.Account;
using GardenHub.Services.Comment;
using GardenHub.Services.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace GardenHub.Web.Controllers
{
    //[Authorize]
    public class CommentController : Controller
    {
        private IAccountService AccountService { get; set; }
        private IPostServices PostServices { get; set; }
        private ICommentServices CommentServices { get; set; }

        public CommentController(IAccountService accountService, IPostServices postServices, ICommentServices commentServices)
        {
            this.AccountService = accountService;

            this.PostServices = postServices;

            this.CommentServices = commentServices;

        }

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return View();
            }
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

        public Comment GetComment(Guid commentId)
        {
            var client = new RestClient();
            var email = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserEmail")).ToString();
            var password = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserPassword")).ToString();

            var requestComment = new RestRequest("https://localhost:5003/api/comment/" + commentId);

            requestComment.AddHeader("Authorization", "Bearer " + GardenHub.Token.Service.Token.Generate(email, password));

            return client.Get<Comment>(requestComment).Data;
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(Guid id)
        {
            var idComment = id;

            return View(this.CommentServices.FindById(idComment));
        }

        // POST: Comment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Guid id, Domain.Comment.Comment comment)
        {
            try
            {
                var commentFromDb = GetComment(comment.Id);
                var email = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserEmail")).ToString();
                var password = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserPassword")).ToString();
                var post = GetPost(new Guid(commentFromDb.PostIdFromRoute.ToString()));

                try
                {
                    var client = new RestClient();
                    var requestComment = new RestRequest("https://localhost:5003/api/comment/edit/");
                    requestComment.AddJsonBody(JsonConvert.SerializeObject(new
                    {
                        Id = commentFromDb.Id,
                        Text = comment.Text
                    }));
                    requestComment.AddHeader("Authorization", "Bearer " + GardenHub.Token.Service.Token.Generate(email, password));

                    await client.PutAsync<Comment>(requestComment);

                    return RedirectToAction("Home", "Post");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine(ex.StackTrace);
                    return RedirectToAction("Home", "Post");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
                return RedirectToAction("Home", "Post");
            }
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(Guid id)
        {
            return View(this.CommentServices.FindById(id));
        }

        // POST: Comment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteComment([FromRoute] Guid id)
        {
            try
            {
                var email = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserEmail")).ToString();
                var password = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserPassword")).ToString();

                var client = new RestClient();
                var requestComment = new RestRequest("https://localhost:5003/api/comment/delete/" + id);
                requestComment.AddHeader("Authorization", "Bearer " + GardenHub.Token.Service.Token.Generate(email, password));

                await client.DeleteAsync<Comment>(requestComment);

                return RedirectToAction("Home", "Post");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
                return RedirectToAction("Home", "Post");
            }
        }
    }
}
