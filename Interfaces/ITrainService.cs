using RailwayReservation.DTOs;
using RailwayReservation.Models;

namespace RailwayReservation.Interfaces;

public interface ITrainService
{
    Task<IEnumerable<TrainResponseDTO>> GetAvailableTrainsAsync(string source, string destination);
    Task<IEnumerable<Train>> GetAllTrainsAsync();
    Task<decimal> CalculateFareAsync(int trainId, int adultCount, int childCount);
    Task<Train> AddTrainAsync(Train train);
    Task<bool> DeleteTrainAsync(int trainId);
}