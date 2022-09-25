using RoommateFinderAPI.Entities.Models;

namespace RoommateFinderAPI.Core
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetRooms(double latitude, double longitude, int maxDistance);
        Task<Room> GetRoom(Guid id);
        void Add(Room room);
        void Remove(Room room);
    }
}