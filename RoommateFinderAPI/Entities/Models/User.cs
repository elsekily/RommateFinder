using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace RoommateFinderAPI.Entities.Models
{
    public class User : IdentityUser
    {
        public ICollection<Room> Rooms { get; set; }
        public User()
        {
            this.Rooms = new Collection<Room>();
        }
    }
}