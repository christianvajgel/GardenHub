using GardenHub.Domain.Post.Repository;
using GardenHub.Repository.Account;
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

        public IAsyncEnumerable<Domain.Post.Post> GetAll()
        {
            //return Context.Posts.AsEnumerable();
            return Context.Posts.Include(x => x.Account).AsAsyncEnumerable();
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
        public async Task<IdentityResult> UpdatePostAsync(Guid postId, Domain.Post.Post newPost)
        {
            var oldPost = Context.Posts.FirstOrDefault(x => x.Id == postId);

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
        public async Task<IdentityResult> DeletePostAsync(Guid postId, Domain.Account.Account account)
        {
            //var post = this.Context.Find<Domain.Post.Post>(postId);
            var post = FindById(postId);
            post.Account = account;

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
