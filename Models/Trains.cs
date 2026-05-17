using System.ComponentModel.DataAnnotations;

namespace RailwayReservation.Models
{
    public class Train
    {
        [Key]
        public int TrainId { get; set; }
        public string TrainNo { get; set; } = null!;
        public string Source { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public string DepartureTime { get; set; } = null!;
        public string ArrivalTime { get; set; } = null!;
        
        public decimal BaseFare { get; set; }
        public int TotalSeats { get; set; }
        public bool IsActive { get; set; } = true; // For Soft Delete
    }
}