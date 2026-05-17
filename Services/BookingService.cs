using RailwayReservation.DTOs;
using RailwayReservation.Interfaces;
using RailwayReservation.Models;

namespace RailwayReservation.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly ITrainRepository _trainRepo;
        private readonly IEmailService _emailService;

        public BookingService(IBookingRepository bookingRepo, ITrainRepository trainRepo, IEmailService emailService)
        {
            _bookingRepo = bookingRepo;
            _trainRepo = trainRepo;
            _emailService = emailService;
        }

        public async Task<string> BookTicketAsync(BookingRequestDTO request, string userId)
        {
            var train = await _trainRepo.GetTrainByIdAsync(request.TrainId);
            if (train == null) throw new Exception("Train not found");

            // Calculate Final Fare using Service Logic
            decimal totalFare = (request.AdultCount * train.BaseFare) + (request.ChildCount * (train.BaseFare * 0.5m));

            // Generate Unique 8-character PNR
            string pnr = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

            var booking = new Booking
            {
                PNR = pnr,
                UserId = userId,
                TrainId = request.TrainId,
                TotalFare = totalFare,
                BookingDate = DateTime.Now,
                Status = "Confirmed",
                Passengers = request.Passengers.Select(p => new Passenger
                {
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender
                }).ToList()
            };

            await _bookingRepo.AddAsync(booking);

            // SRP: Trigger notification after DB success
            await _emailService.SendEmailAsync(userId, "Booking Confirmed", $"Your PNR is {pnr}. Total Fare: {totalFare} PKR.");

            return pnr;
        }

        public async Task<bool> CancelBookingAsync(string pnr)
        {
            var booking = await _bookingRepo.GetByPnrAsync(pnr);
            if (booking == null) return false;

            booking.Status = "Cancelled";
            await _bookingRepo.UpdateAsync(booking);
            
            return true;
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId) => await _bookingRepo.GetByUserIdAsync(userId);
        public async Task<Booking?> GetBookingByPnrAsync(string pnr) => await _bookingRepo.GetByPnrAsync(pnr);
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync() => await _bookingRepo.GetAllAsync();
    }
}