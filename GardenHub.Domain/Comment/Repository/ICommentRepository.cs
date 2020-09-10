using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GardenHub.Domain.Comment.Repository
{
    public interface ICommentRepository
    {
        Task<IdentityResult> CreateCommentAsync(Domain.Comment.Comment comment);
        Task<IdentityResult> UpdateCommentAsync(Domain.Comment.Comment comment);
        Task<IdentityResult> FindByIdAsync(Guid commentId);
        Task<IdentityResult> DeleteCommentAsync(Domain.Comment.Comment comment);
    }
}
