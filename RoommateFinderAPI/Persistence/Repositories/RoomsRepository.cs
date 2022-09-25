using Microsoft.EntityFrameworkCore;
using RoommateFinderAPI.Core;
using RoommateFinderAPI.Entities.Models;
using RoommateFinderAPI.Entities.Resources;

namespace RoommateFinderAPI.Persistence.Repositories
{
    public class Test
    {
        public Guid Id { get; set; }
        public double Distance { get; set; }
    }
    public class RoomsRepository : IRoomRepository
    {
        private readonly RoommateFinderDbContext context;
        public RoomsRepository(RoommateFinderDbContext context)
        {
            this.context = context;
        }
        public async void Add(Room room)
        {
            await context.Rooms.AddAsync(room);
        }

        public async Task<Room> GetRoom(Guid id)
        {
            return await context.Rooms.Where(r => r.Id == id)
            .Include(r => r.RoomTags).ThenInclude(rt => rt.Tag).Include(r => r.Owner).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Room>> GetRooms(double latitude, double longitude, int maxDistance)
        {
            var userLocation = LocationUtilities.GetLocation(latitude, longitude);
            return await context.Rooms.Where(r => r.Location.Distance(userLocation) * 100 <= maxDistance)//in km
            .Include(r => r.RoomTags).ThenInclude(rt => rt.Tag).Include(r => r.Owner)
            .OrderBy(r => r.Location.Distance(userLocation)).ToListAsync();
            //
        }

        public void Remove(Room room)
        {
            context.Rooms.Remove(room);
        }
    }
}