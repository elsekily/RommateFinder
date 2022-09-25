using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoommateFinderAPI.Entities.Models;

namespace RoommateFinderAPI.Persistence
{
    public class RoommateFinderDbContext : IdentityDbContext<User>
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public RoommateFinderDbContext(DbContextOptions<RoommateFinderDbContext> options)
        : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Room>().HasOne(r => r.Owner).WithMany(u => u.Rooms).HasForeignKey(r => r.UserId);
            builder.Entity<Room>().Property(c => c.Location).HasSrid(4326);

            builder.Entity<RoomTag>().HasKey(rt => new { rt.RoomId, rt.TagId });
            builder.Entity<RoomTag>().HasOne(rt => rt.Room).WithMany(r => r.RoomTags).HasForeignKey(rt => rt.RoomId);
            builder.Entity<RoomTag>().HasOne(rt => rt.Tag).WithMany(r => r.RoomTags).HasForeignKey(rt => rt.TagId);
        }
    }
}