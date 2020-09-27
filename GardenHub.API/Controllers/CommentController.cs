using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GardenHub.Domain.Comment;
using GardenHub.Services.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GardenHub.API.Controllers
{
    //[Authorize]
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentServices CommentServices { get; set; }

        public CommentController(ICommentServices commentServices)
        {
            this.CommentServices = commentServices;
        }

        // GET: api/<CommentController>
        [HttpGet("get")]
        public IEnumerable<Comment> Get()
        {
            return this.CommentServices.GetAll();
        }

        // GET api/<CommentController>/5
        [HttpGet("{id}")]
        public Comment Get(Guid id)
        {
            return this.CommentServices.FindById(id);
        }

        // POST api/<CommentController>
        [HttpPost("create")]
        //[HttpComment]
        public async Task<IdentityResult> Post([FromBody] Comment comment)
        {
            return await this.CommentServices.SaveComment(comment);
        }

        // PUT api/<CommentController>/5
        [HttpPut("edit")]
        public async Task<IdentityResult> Put([FromBody] Comment comment)
        {
            return await this.CommentServices.EditComment(comment);
        }

        // DELETE api/<CommentController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IdentityResult> Delete([FromRoute] Guid id)
        {
            return await this.CommentServices.DeleteComment(id);
        }
    }
}
