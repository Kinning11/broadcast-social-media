using BroadcastSocialMedia.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BroadcastSocialMedia.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Broadcast> Broadcasts { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Broadcast>()
                .HasOne(b => b.User)
                .WithMany(u => u.Broadcasts)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
