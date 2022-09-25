using System.Collections.ObjectModel;
using NetTopologySuite.Geometries;

namespace RoommateFinderAPI.Entities.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public Point Location { get; set; }
        public int NumberofPeopleinTheRoom { get; set; }
        public int NumberofPeopleintheApartment { get; set; }
        public int NumberofBathRoomsintheApartment { get; set; }
        public double RentPerMounth { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }
        public User Owner { get; set; }
        public ICollection<RoomTag> RoomTags { get; set; }
        public Room()
        {
            this.RoomTags = new Collection<RoomTag>();
        }
    }
}