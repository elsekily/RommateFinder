namespace RoommateFinderAPI.Entities.Models
{
    public class RoomTag
    {
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}