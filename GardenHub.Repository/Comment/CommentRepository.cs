using GardenHub.Domain.Comment.Repository;
using GardenHub.Repository.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace GardenHub.Repository.Comment
{
    public class CommentRepository : ICommentRepository
    {
        private GardenHubContext Context { get; set; }

        public CommentRepository(GardenHubContext gardenHubContext)
        {
            this.Context = gardenHubContext;
        }

        // CREATE
        public Task<IdentityResult> CreateCommentAsync(Domain.Comment.Comment comment)
        {
            throw new NotImplementedException();
        }

        // READ
        public Task<IdentityResult> FindByIdAsync(Guid commentId)
        {
            throw new NotImplementedException();
        }

        // UPDATE
        public Task<IdentityResult> UpdateCommentAsync(Domain.Comment.Comment comment)
        {
            throw new NotImplementedException();
        }

        // DELETE
        public Task<IdentityResult> DeleteCommentAsync(Domain.Comment.Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
