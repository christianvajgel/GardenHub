using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GardenHub.Repository.Mapping
{
    public class PostMap : IEntityTypeConfiguration<Domain.Post.Post>
    {
        public void Configure(EntityTypeBuilder<Domain.Post.Post> builder)
        {
            builder.ToTable("Post");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Image).HasMaxLength(250);
            builder.Property(x => x.Description).HasMaxLength(500);

            // One ACCOUNT to Many POSTS
            builder.HasOne<Domain.Account.Account>(x => x.Account);

            // Many COMMENTS to One POST
            builder.HasMany<Domain.Comment.Comment>(x => x.Comments).WithOne(x => x.Post);

            // Forget this code below
            //builder.HasOne(x => x.Account).WithMany(x => x.Posts);
        }
    }
}
