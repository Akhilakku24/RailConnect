using System.ComponentModel.DataAnnotations;

namespace RailwayReservation.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public string PNR { get; set; } = null!;
        public string UserId { get; set; } = null!; // Identity User Link
        public int TrainId { get; set; }
        public decimal TotalFare { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled
        
        // Relationship: One Booking has many Passengers
        public List<Passenger> Passengers { get; set; } = new();
    }
}