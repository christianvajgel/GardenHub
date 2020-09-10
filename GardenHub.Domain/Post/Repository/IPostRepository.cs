using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GardenHub.Domain.Post.Repository
{
    public interface IPostRepository
    {
        Task<IdentityResult> CreatePostAsync(Domain.Post.Post post);
        //Task<IdentityResult> UpdatePostAsync(Domain.Post.Post post);
        Task<IdentityResult> UpdatePostAsync(Guid postId, Domain.Post.Post newPost);
        //Task<IdentityResult> FindByIdAsync(Guid postId);

        Domain.Post.Post FindById(Guid id);

        Task<IdentityResult> DeletePostAsync(Guid id, Domain.Account.Account account);

        IAsyncEnumerable<Domain.Post.Post> GetAll();
    }
}
