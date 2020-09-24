using GardenHub.Domain.Comment.Repository;
using GardenHub.Repository.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public IAsyncEnumerable<Domain.Comment.Comment> GetAll()
        {
            //return Context.Posts.AsEnumerable();
            return Context.Comments.Include(x => x.Post).AsAsyncEnumerable();
        }

        // CREATE
        public async Task<IdentityResult> CreateCommentAsync(Domain.Comment.Comment comment)
        {
            comment.Id = Guid.NewGuid();
            this.Context.Comments.Add(comment);
            await this.Context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        // READ

        public async Task<GardenHub.Domain.Comment.Comment> FindByIdAsync(Guid commentId)
        {
            var comment = await this.Context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
            return comment;
        }

        public Domain.Comment.Comment FindById(Guid commentId)
        {
            var comment = this.Context.Comments.FirstOrDefault(x => x.Id == commentId);
            return comment;
        }

        /*public Task<GardenHub.Domain.Comment.Comment> FindAllByIdPost(Guid postId)
        {
            var comments = new List<GardenHub.Domain.Comment.Comment>();
            foreach (var comment in this.Context.Comments.Where(x => x.Post.Id == postId))
            {
                comments.Add(comment);
            }
            return comments;
        }*/

        // UPDATE
        public async Task<IdentityResult> UpdateCommentAsync(Guid id, Domain.Comment.Comment newComment)
        {
            var oldComment = FindById(id);

            oldComment.Text = newComment.Text;

            Context.Comments.Update(oldComment);
            await this.Context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        // DELETE
        public async Task<IdentityResult> DeleteCommentAsync(Guid id)
        {
            var commentToDelete = FindById(id);
            
            this.Context.Comments.Remove(commentToDelete);
            await this.Context.SaveChangesAsync();
            return IdentityResult.Success;
        }


    }
}
