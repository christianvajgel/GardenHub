using System.Threading.Tasks;

namespace GardenHub.Services.Comment
{
    public interface ICommentServices
    {
        Task SaveComment(GardenHub.Domain.Comment.Comment comment);
    }
}
