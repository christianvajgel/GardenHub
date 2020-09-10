using System;
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

        public Task SaveComment(Domain.Comment.Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
