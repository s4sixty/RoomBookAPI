using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are accepted in this field")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are accepted in this field")]
        public string LastName { get; set; }

        public List<Reservation> reservations { get; set; } = new List<Reservation>();
    }
}
