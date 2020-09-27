using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GardenHub.Domain.Comment.Repository
{
    public interface ICommentRepository
    {
        Task<IdentityResult> CreateCommentAsync(Domain.Comment.Comment post);

        Task<IdentityResult> UpdateCommentAsync(Guid postId, Domain.Comment.Comment newComment);

        Task<IdentityResult> UpdateCommentAsync(Domain.Comment.Comment newComment);

        Domain.Comment.Comment FindById(Guid id);

        Task<IdentityResult> DeleteCommentAsync(Guid id);

        IEnumerable<Domain.Comment.Comment> GetAll();
    }
}
