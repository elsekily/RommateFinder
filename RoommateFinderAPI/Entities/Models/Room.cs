using System.Collections.ObjectModel;

namespace RoommateFinderAPI.Entities.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double longitude { get; set; }
        public int NumberofPeopleinTheRoom { get; set; }
        public int NumberofPeopleintheApartment { get; set; }
        public double RentPerMounth { get; set; }
        public string UserId { get; set; }
        public User PublishedBy { get; set; }
        public ICollection<RoomTag> RoomTags { get; set; }
        public Room()
        {
            this.RoomTags = new Collection<RoomTag>();
        }
    }
}