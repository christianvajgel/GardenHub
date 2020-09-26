using GardenHub.Domain.Post.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GardenHub.Services.Post
{
    public class PostServices : IPostServices
    {
        private IPostRepository PostRepository { get; set; }

        public PostServices(IPostRepository postRepository)
        {
            this.PostRepository = postRepository;
        }

        public async Task<IdentityResult> SavePost(Domain.Post.Post post)
        {
            return await PostRepository.CreatePostAsync(post);
        }

        public Domain.Post.Post FindById(Guid id) 
        {
            return PostRepository.FindById(id);
        }

        //public async Task EditPost(Guid postId, Domain.Post.Post newPost)
        public async Task<IdentityResult> EditPost(Domain.Post.Post newPost)
        {
            return await PostRepository.UpdatePostAsync(newPost);
            //await PostRepository.UpdatePostAsync(postId, newPost);
        }

        //public async Task DeletePost(Guid postId, Domain.Account.Account account)
        public async Task<IdentityResult> DeletePost(Guid postId)
        {
            return await PostRepository.DeletePostAsync(postId);
        }

        public IEnumerable<Domain.Post.Post> GetAll()
        {
            return PostRepository.GetAll();
        }
    }
}
