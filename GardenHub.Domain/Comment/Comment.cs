using System;
using System.Text.Json.Serialization;

namespace GardenHub.Domain.Comment
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid AccountOwnerId { get; set; }
        public DateTime PostedTime { get; set; }
        public Guid PostIdFromRoute { get; set; }

        // Reference POST to COMMENTS
        [JsonIgnore]
        public virtual Domain.Post.Post Post { get; set; }
    }
}
