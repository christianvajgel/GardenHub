using GardenHub.Domain.Post.Repository;
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

        public async Task SavePost(Domain.Post.Post post)
        {
            await PostRepository.CreatePostAsync(post);
        }

        public Domain.Post.Post FindById(Guid id) 
        {
            return PostRepository.FindById(id);
        }

        public async Task EditPost(Guid postId, Domain.Post.Post newPost)
        {
            await PostRepository.UpdatePostAsync(postId, newPost);
        }

        public async Task DeletePost(Guid postId, Domain.Account.Account account)
        {
            await PostRepository.DeletePostAsync(postId, account);
        }

        public IAsyncEnumerable<Domain.Post.Post> GetAll()
        {
            return PostRepository.GetAll();
        }
    }
}
