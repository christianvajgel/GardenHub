using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GardenHub.CrossCutting.Storage;
using GardenHub.Services.Account;
using GardenHub.Services.Comment;
using GardenHub.Services.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GardenHub.Web.Controllers
{
    public class CommentController : Controller
    {
        private IAccountService AccountService { get; set; }
        private IPostServices PostServices { get; set; }
        private ICommentServices CommentServices { get; set; }

        private AzureStorage AzureStorage { get; set; }

        public CommentController(IAccountService accountService, IPostServices postServices, ICommentServices commentServices, AzureStorage azureStorage)
        //public PostController(IPostServices postServices)
        {
            this.AccountService = accountService;

            this.PostServices = postServices;

            this.CommentServices = commentServices;

            this.AzureStorage = azureStorage;

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

        // GET: Comment/Edit/5
        public ActionResult Edit(Guid id)
        {
            var idComment = id;
           
            return View(this.CommentServices.FindById(idComment));
        }

        // POST: Comment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, Domain.Comment.Comment commentOld)
        {
            try
            {
                //var comment = this.CommentServices.FindById(id);
                CommentServices.EditComment(id, commentOld);

                return Redirect("../../post/Home");
            }
            catch
            {
                return View();
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
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                var comment = this.CommentServices.FindById(id);

                this.CommentServices.DeleteComment(id);

                return Redirect("../../post/Home");
            }
            catch
            {
                return View();
            }
        }
    }
}