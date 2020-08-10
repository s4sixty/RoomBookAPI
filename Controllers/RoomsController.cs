using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookAPI.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class RoomsController: Controller
    {
        // Inject the DBContext into the controller...
        private ApiContext _context;

        public RoomsController(ApiContext context)
        {
            _context = context;
        }

        // Return a list of all available rooms
        [HttpGet]
        public async Task<List<Room>> GetRooms()
        {
            var rooms = await _context.Rooms.AsNoTracking().ToListAsync();
            return rooms;
        }

        // Return a single room from a given id
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            Room room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return room;
        }
    }
}
