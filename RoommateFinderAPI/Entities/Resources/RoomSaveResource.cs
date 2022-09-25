using System.ComponentModel.DataAnnotations;

namespace RoommateFinderAPI.Entities.Resources
{
    public class RoomSaveResource
    {
        [Required]
        [MaxLength(500)]
        public string Address { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public int NumberofPeopleinTheRoom { get; set; }
        [Required]
        public int NumberofPeopleintheApartment { get; set; }
        [Required]
        public int NumberofBathRoomsintheApartment { get; set; }
        [Required]
        public double RentPerMounth { get; set; }
        public string Notes { get; set; }
        public IList<Guid> TagIds { get; set; }
        public RoomSaveResource()
        {
            this.TagIds = new List<Guid>();
        }
    }
}