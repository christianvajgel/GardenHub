using GardenHub.Domain.Post.Repository;
using GardenHub.Repository.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenHub.Repository.Post
{
    public class PostRepository : IPostRepository
    {

        private GardenHubContext Context { get; set; }

        public PostRepository(GardenHubContext gardenHubContext)
        {
            this.Context = gardenHubContext;
        }

        public IEnumerable<Domain.Post.Post> GetAll()
        {
            var r = Context.Posts.Include(x => x.Account).AsEnumerable();
            return r;
        }

        // CREATE
        public async Task<IdentityResult> CreatePostAsync(Domain.Post.Post post)
        {
            this.Context.Posts.Add(post);
            await this.Context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        // READ
        public Domain.Post.Post FindById(Guid postId)
        {
            return this.Context.Posts.Include(x => x.Comments).FirstOrDefault(x => x.Id == postId);
        }

        // UPDATE
        public async Task<IdentityResult> UpdatePostAsync(Domain.Post.Post newPost)
        {
            var oldPost = Context.Posts.FirstOrDefault(x => x.Id == newPost.Id);

            if (String.IsNullOrWhiteSpace(newPost.Image) && String.IsNullOrWhiteSpace(newPost.AzureFilename))
            {
                oldPost.Image = null;
                oldPost.AzureFilename = null;
            }
            else
            {
                oldPost.Image = newPost.Image;
                oldPost.AzureFilename = newPost.AzureFilename;
            }

            oldPost.Description = newPost.Description;

            Context.Posts.Update(oldPost);
            await this.Context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        // DELETE
        public async Task<IdentityResult> DeletePostAsync(Guid postId)
        {
            var post = FindById(postId);

            if (post.Comments != null)
            {
                foreach (var item in post.Comments)
                {
                    Context.Comments.Remove(item);
                }
            }

            this.Context.Posts.Remove(post);
            await this.Context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
