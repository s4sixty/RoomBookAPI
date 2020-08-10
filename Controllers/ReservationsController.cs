using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookAPI.Models;

namespace RoomBookAPI.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ReservationsController : Controller
    {
        // Inject the DBContext into the controller...
        private ApiContext _context;
        // The variable UserClaims will store data from the authorized JWT
        public ClaimsPrincipal UserClaims { get; }

        public ReservationsController(ApiContext context)
        {
            _context = context;
        }

        // Return a list of reservations for the user id in the token
        [HttpGet]
        public async Task<ActionResult<Reservation>> GetReservations()
        {
            int idClaims = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            User user = await _context.Users.FindAsync(idClaims);
            if (user == null)
            {
                return NotFound();
            }
            var reservations = await _context.Reservations.Where(c => c.UserID == user.Id).ToListAsync();
            return Ok(reservations);
        }

        // Create a reservation for the user id in the token
        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation([FromBody] Reservation data)
        {
            int idClaims = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            User user = await _context.Users.FindAsync(idClaims);
            if (user == null)
            {
                return NotFound();
            }

            if(data.endTime.Date != data.beginTime.Date)
            {
                return BadRequest(new { message = "Your reservation must not bypass one day." });
            }

            if(_context.Reservations.FirstOrDefault(c => c.RoomID == data.RoomID) != null)
            {
                return BadRequest(new { message = "Your have already booked this room !" });
            }

            if (_context.Reservations.FirstOrDefault(c => (c.beginTime.Ticks > data.beginTime.Ticks && c.beginTime.Ticks < data.endTime.Ticks)
            || (c.endTime.Ticks > data.beginTime.Ticks && c.endTime.Ticks < data.endTime.Ticks)) != null)
            {
                return BadRequest(new { message = "This room has already been booked, please choose another date !" });
            }

            Console.WriteLine(data.beginTime);
            Console.WriteLine(data.endTime);

            Reservation reservation = new Reservation
            {
                RoomID = data.RoomID,
                UserID = user.Id,
                beginTime = data.beginTime,
                endTime = data.endTime
            };
            user.reservations.Add(reservation);

            _context.SaveChanges();

            return Ok(new {
                message = "Reservation succeeded",
                reservation = reservation
            });
        }
    }
}
