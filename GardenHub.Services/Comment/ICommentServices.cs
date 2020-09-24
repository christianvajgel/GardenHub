using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GardenHub.Services.Comment
{
    public interface ICommentServices
    {
        Task SaveComment(GardenHub.Domain.Comment.Comment comment);
        Task FindByIdAsync(Guid idComment);
        Domain.Comment.Comment FindById(Guid idComment);
        IAsyncEnumerable<Domain.Comment.Comment> GetAll();
        Task DeleteComment(Guid commentId);
        Task EditComment(Guid commentId, Domain.Comment.Comment newComment);
    }
}
