using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RoomBookAPI.Models
{
    public class Reservation
    {
        [Key]
        public int RoomID { get; set; }
        [Key]
        public int UserID { get; set; }

        [JsonIgnore]
        public Room room { get; set; }
        [JsonIgnore]
        public User user { get; set; }

        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
