using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GardenHub.Domain.Comment.Repository;

namespace GardenHub.Services.Comment
{
    public class CommentServices : ICommentServices
    {
        private ICommentRepository CommentRepository { get; set; }

        public CommentServices(ICommentRepository accountRepository)
        {
            this.CommentRepository = accountRepository;
        }

        public async Task SaveComment(Domain.Comment.Comment comment)
        {
             await CommentRepository.CreateCommentAsync(comment);
        }

        public async Task FindByIdAsync(Guid idComment)
        {
            await CommentRepository.FindByIdAsync(idComment);
        }

       
        public IAsyncEnumerable<Domain.Comment.Comment> GetAll()
        {
            return CommentRepository.GetAll();
        }

        public Domain.Comment.Comment FindById(Guid idComment)
        {
            return CommentRepository.FindById(idComment);
        }

        public async Task DeleteComment(Guid commentId)
        {
            await CommentRepository.DeleteCommentAsync(commentId);
        }

        public async Task EditComment(Guid commentId, Domain.Comment.Comment newComment)
        {
            await CommentRepository.UpdateCommentAsync(commentId,newComment);
        }
    }
}
