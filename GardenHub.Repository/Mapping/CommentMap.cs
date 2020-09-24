using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GardenHub.Repository.Mapping
{
    public class CommentMap : IEntityTypeConfiguration<Domain.Comment.Comment>
    {
        public void Configure(EntityTypeBuilder<Domain.Comment.Comment> builder)
        {
            builder.ToTable("Comment");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Text).IsRequired().HasMaxLength(500);
            builder.Property(x => x.AccountOwnerId).IsRequired();
            builder.Property(x => x.PostedTime).IsRequired();

            // Many COMMENTS to One POST
            builder.HasOne<Domain.Post.Post>(x => x.Post);
            

            // Forget this code below
            //builder.HasOne(x => x.Post).WithMany(x => x.Comments);
        }
    }
}
