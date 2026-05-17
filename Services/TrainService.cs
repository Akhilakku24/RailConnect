using RailwayReservation.DTOs;
using RailwayReservation.Interfaces;
using RailwayReservation.Models;

namespace RailwayReservation.Services
{
    public class TrainService : ITrainService
    {
        private readonly ITrainRepository _trainRepo;

        public TrainService(ITrainRepository trainRepo)
        {
            _trainRepo = trainRepo;
        }

        public async Task<IEnumerable<TrainResponseDTO>> GetAvailableTrainsAsync(string source, string destination)
        {
            var trains = await _trainRepo.GetAllTrainsAsync();
            
            // Filter by route and map to DTO
            return trains
                .Where(t => t.Source.Equals(source, StringComparison.OrdinalIgnoreCase) && 
                            t.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase))
                .Select(t => new TrainResponseDTO
                {
                    TrainId = t.TrainId,
                    TrainNo = t.TrainNo,
                    Source = t.Source,
                    Destination = t.Destination,
                    ArrivalTime = t.ArrivalTime,
                    DepartureTime = t.DepartureTime,
                    BaseFare = t.BaseFare,
                    AvailableSeats = t.TotalSeats // In a real app, subtract current bookings here
                });
        }

        public async Task<decimal> CalculateFareAsync(int trainId, int adultCount, int childCount)
        {
            var train = await _trainRepo.GetTrainByIdAsync(trainId);
            if (train == null) return 0;

            // SRP: Business Rule - Adults pay full, Children pay 50%
            decimal adultTotal = adultCount * train.BaseFare;
            decimal childTotal = childCount * (train.BaseFare * 0.5m);

            return adultTotal + childTotal;
        }

        public async Task<Train> AddTrainAsync(Train train)
        {
            train.IsActive = true;
            await _trainRepo.AddTrainAsync(train);
            return train;
        }

        public async Task<bool> DeleteTrainAsync(int trainId)
        {
            await _trainRepo.DeleteTrainAsync(trainId);
            return true;
        }

        public async Task<IEnumerable<Train>> GetAllTrainsAsync()
        {
            return await _trainRepo.GetAllTrainsAsync();
        }
    }
}