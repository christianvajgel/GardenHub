using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenHub.Web.ViewModel.Post
{
    public class HomePostViewModel
    {
        public IList<GardenHub.Domain.Post.Post> Posts { get; set; }
        public GardenHub.Domain.Account.Account Account { get; set; }

    }
}
