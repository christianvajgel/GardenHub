using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GardenHub.Repository.Mapping;

namespace GardenHub.Repository.Context
{
    public class GardenHubContext : DbContext
    {

        public DbSet<Domain.Account.Account> Accounts { get; set; }
        public DbSet<Domain.Role> Profiles { get; set; }

        public DbSet<Domain.Post.Post> Posts { get; set; }
        public DbSet<Domain.Comment.Comment> Comments { get; set; }
        

        public static readonly ILoggerFactory _loggerFactory
                    = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public GardenHubContext(DbContextOptions<GardenHubContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountMap());
            modelBuilder.ApplyConfiguration(new RoleMap());

            modelBuilder.ApplyConfiguration(new PostMap());
            modelBuilder.ApplyConfiguration(new CommentMap());

            base.OnModelCreating(modelBuilder);
        }

    }
}
