namespace RoommateFinderAPI.Entities.Models
{
    public class RoomTag
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}