namespace RailwayReservation.DTOs
{
    public class BookingRequestDTO
    {
        public int TrainId { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public string ClassType { get; set; } = null!; // Economy, Business, or AC
        public string Quota { get; set; } = "General";
        public string ContactAddress { get; set; } = null!;
        public string CreditCardNo { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public DateTime JourneyDate { get; set; }
        public List<PassengerDTO> Passengers { get; set; } = new();
    }
    public class PassengerDTO
{
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Gender { get; set; } = null!;
}
}