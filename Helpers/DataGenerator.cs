using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoomBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookAPI.Helpers
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApiContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApiContext>>()
                ))
            {
                // Id data has already been seeded return
                if (context.Rooms.Any() || context.Users.Any()) return;

                // Adding 10 rooms for sample data
                context.Rooms.Add(new Room { Name = "room0" });
                context.Rooms.Add(new Room { Name = "room1" });
                context.Rooms.Add(new Room { Name = "room2" });
                context.Rooms.Add(new Room { Name = "room3" });
                context.Rooms.Add(new Room { Name = "room4" });
                context.Rooms.Add(new Room { Name = "room5" });
                context.Rooms.Add(new Room { Name = "room6" });
                context.Rooms.Add(new Room { Name = "room7" });
                context.Rooms.Add(new Room { Name = "room8" });
                context.Rooms.Add(new Room { Name = "room9" });

                // Adding a few users for sample data
                context.Users.Add(new User { Username = "ninja", Password = "ninja", FirstName = "Samir", LastName = "Amara" });
                context.Users.Add(new User { Username = "admin", Password = "admin", FirstName = "admin", LastName = "admin" });
                context.Users.Add(new User { Username = "test", Password = "test", FirstName = "test", LastName = "test" });


                context.SaveChanges();
            }
        }
    }
}
