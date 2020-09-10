﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GardenHub.Domain.Comment
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid Owner { get; set; }

        // Reference POST to COMMENTS
        [JsonIgnore]
        public virtual Domain.Post.Post Post { get; set; }

        //public Domain.Post.Post Post { get; set; }

        // Reference POST to COMMENT
        //public Domain.Post.Post PostId { get; set; }
        //public Domain.Post.Post Post { get; set; }
    }
}
