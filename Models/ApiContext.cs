using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookAPI.Models
{
    public class ApiContext: DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
            {
            }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasKey(x => new { x.UserID, x.RoomID });

            modelBuilder.Entity<Reservation>()
                .HasOne(pt => pt.room)
                .WithMany(p => p.reservations)
                .HasForeignKey(pt => pt.RoomID);

            modelBuilder.Entity<Reservation>()
                .HasOne(pt => pt.user)
                .WithMany(t => t.reservations)
                .HasForeignKey(pt => pt.UserID);
        }
    }
}
