using System.ComponentModel.DataAnnotations;

namespace RailwayReservation.Models
{
    public class Passenger
    {
        [Key]
        public int PassengerId { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        
        // Foreign Key back to Booking
        public int BookingId { get; set; }
    }
}