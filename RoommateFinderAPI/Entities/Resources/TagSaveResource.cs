using System.ComponentModel.DataAnnotations;

namespace RoommateFinderAPI.Entities.Resources
{
    public class TagSaveResource
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}