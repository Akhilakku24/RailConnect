using RailwayReservation.Models;
using RailwayReservation.DTOs;

namespace RailwayReservation.Interfaces;

public interface IBookingService
{
    Task<string> BookTicketAsync(BookingRequestDTO request, string userId);
    Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId);
    Task<Booking?> GetBookingByPnrAsync(string pnr);
    Task<bool> CancelBookingAsync(string pnr);
    Task<IEnumerable<Booking>> GetAllBookingsAsync();
}