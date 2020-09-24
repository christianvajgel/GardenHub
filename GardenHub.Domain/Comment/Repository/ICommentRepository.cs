using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GardenHub.Domain.Comment.Repository
{
    public interface ICommentRepository
    {
        Task<IdentityResult> CreateCommentAsync(Domain.Comment.Comment comment);
        Task<IdentityResult> UpdateCommentAsync(Guid id, Domain.Comment.Comment comment);
        Task<GardenHub.Domain.Comment.Comment> FindByIdAsync(Guid commentId);
        Task<IdentityResult> DeleteCommentAsync(Guid commentId);
        IAsyncEnumerable<Domain.Comment.Comment> GetAll();
        GardenHub.Domain.Comment.Comment FindById(Guid commentId);
    }
}
