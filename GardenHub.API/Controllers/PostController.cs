using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GardenHub.Domain.Post;
using GardenHub.Services.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GardenHub.API.Controllers
{
    //[Authorize]
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private IPostServices PostServices { get; set; }

        public PostController(IPostServices postServices)
        {
            this.PostServices = postServices;
        }

        // GET: api/<PostController>
        [HttpGet("get")]
        public IEnumerable<Post> Get()
        {
            return this.PostServices.GetAll();
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public Post Get(Guid id)
        {
            return this.PostServices.FindById(id);
        }

        // POST api/<PostController>
        [HttpPost("create")]
        //[HttpPost]
        public async Task<IdentityResult> Post([FromBody] Post post)
        {
            return await this.PostServices.SavePost(post);
        }

        // PUT api/<PostController>/5
        [HttpPut("edit")]
        public async Task<IdentityResult> Put([FromBody] Post post)
        {
            return await this.PostServices.EditPost(post);
        }

        // DELETE api/<PostController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IdentityResult> Delete([FromRoute] Guid id)
        {
            return await this.PostServices.DeletePost(id);
        }
    }
}
