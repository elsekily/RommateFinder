using System.Collections.ObjectModel;

namespace RoommateFinderAPI.Entities.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoomTag> RoomTags { get; set; }
        public Tag()
        {
            this.RoomTags = new Collection<RoomTag>();

        }
    }
}