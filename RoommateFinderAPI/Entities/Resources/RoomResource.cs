namespace RoommateFinderAPI.Entities.Resources
{
    public class RoomResource
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public int NumberofPeopleinTheRoom { get; set; }
        public int NumberofPeopleintheApartment { get; set; }
        public int NumberofBathRoomsintheApartment { get; set; }
        public double RentPerMounth { get; set; }
        public string Notes { get; set; }
        public UserResource Owner { get; set; }
        public IList<TagResource> Tags { get; set; }
        public RoomResource()
        {
            Tags = new List<TagResource>();
        }
    }
}