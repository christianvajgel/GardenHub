using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GardenHub.Domain.Comment.Repository;
using Microsoft.AspNetCore.Identity;

namespace GardenHub.Services.Comment
{
    public class CommentServices : ICommentServices
    {
        private ICommentRepository CommentRepository { get; set; }

        public CommentServices(ICommentRepository accountRepository)
        {
            this.CommentRepository = accountRepository;
        }

        public async Task<IdentityResult> SaveComment(Domain.Comment.Comment comment)
        {
            return await CommentRepository.CreateCommentAsync(comment);
        }

        public IEnumerable<Domain.Comment.Comment> GetAll()
        {
            return CommentRepository.GetAll();
        }

        public Domain.Comment.Comment FindById(Guid idComment)
        {
            return CommentRepository.FindById(idComment);
        }

        public async Task<IdentityResult> DeleteComment(Guid commentId)
        {
            return await CommentRepository.DeleteCommentAsync(commentId);
        }

        public async Task<IdentityResult> EditComment(Domain.Comment.Comment newComment)
        {
            return await CommentRepository.UpdateCommentAsync(newComment);
        }

        public async Task EditComment(Guid commentId, Domain.Comment.Comment newComment)
        {
            await CommentRepository.UpdateCommentAsync(commentId, newComment);
        }
    }
}
