using GardenHub.Domain.Account.Repository;
using GardenHub.Domain.Post.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenHub.Services.Post
{
    public class PostServices : IPostServices
    {
        private IPostRepository PostRepository { get; set; }
        private IAccountRepository AccountRepository { get; set; }

        public PostServices(IPostRepository postRepository, IAccountRepository accountRepository)
        {
            this.PostRepository = postRepository;
            this.AccountRepository = accountRepository;
        }

        public async Task<IdentityResult> SavePost(Domain.Post.Post post)
        {
            post.Account = AccountRepository.GetAll().Where(x => x.Email == post.AccountEmail).FirstOrDefault();
            return await PostRepository.CreatePostAsync(post);
        }

        public Domain.Post.Post FindById(Guid id)
        {
            return PostRepository.FindById(id);
        }

        public async Task<IdentityResult> EditPost(Domain.Post.Post newPost)
        {
            return await PostRepository.UpdatePostAsync(newPost);
        }

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
