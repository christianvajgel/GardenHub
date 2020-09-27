using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GardenHub.Domain.Post
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string AzureFilename { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string AccountEmail { get; set; }

        // Reference ACCOUNT to POSTS
        [JsonIgnore]
        public virtual Domain.Account.Account Account { get; set; }

        // Reference POST to COMMENTS
        public virtual IList<Domain.Comment.Comment> Comments { get; set; }
    }
}
