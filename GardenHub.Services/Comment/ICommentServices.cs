using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GardenHub.Services.Comment
{
    public interface ICommentServices
    {
        Task<IdentityResult> SaveComment(GardenHub.Domain.Comment.Comment comment);

        Domain.Comment.Comment FindById(Guid id);

        Task EditComment(Guid commentId, Domain.Comment.Comment newComment);

        Task<IdentityResult> EditComment(Domain.Comment.Comment newComment);

        Task<IdentityResult> DeleteComment(Guid commentId);

        IEnumerable<Domain.Comment.Comment> GetAll();
    }
}
