using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenHub.Services.Post
{
    public interface IPostServices
    {
        Task<IdentityResult> SavePost(GardenHub.Domain.Post.Post post);
        Domain.Post.Post FindById(Guid id);
        //Task EditPost(Guid postId, Domain.Post.Post newPost);
        Task<IdentityResult> EditPost(Domain.Post.Post newPost);
        //Task DeletePost(Guid postId, Domain.Account.Account account);
        //Task DeletePost(Guid postId);
        Task<IdentityResult> DeletePost(Guid postId);
        IEnumerable<Domain.Post.Post> GetAll();

    }
}
