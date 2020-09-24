using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenHub.Web.ViewModel.Post
{
    public class PostViewModel
    {

        public GardenHub.Domain.Account.Account Account { get; set; }
        public GardenHub.Domain.Post.Post Post { get; set; }
        public GardenHub.Domain.Comment.Comment Comment { get; set; }
        public List<GardenHub.Domain.Comment.Comment> Comments { get; set; }
    }
}
