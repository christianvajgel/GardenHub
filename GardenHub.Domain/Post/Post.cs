using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GardenHub.Domain.Post
{
    public enum PostType 
    {
        Image, 
        Text, 
        ImageText
    }

    public class Post
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string AzureFilename { get; set; }
        public string Description { get; set; }
        public PostType Type { get; set; } 

        // Reference ACCOUNT to POSTS
        [JsonIgnore]
        public virtual Domain.Account.Account Account { get; set; }

        // Reference POST to COMMENTS
        public virtual IList<Domain.Comment.Comment> Comments { get; set; }


        // Forget this code below

        //public List<Domain.Comment.Comment> Comments { get; set; }

        //public Domain.Account.Account Account { get; set; }

        // Reference USER to POST
        //public Domain.Account.Account AccountId { get; set; }
        //public Domain.Account.Account Account { get; set; }

    }
}
