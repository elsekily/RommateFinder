using RoommateFinderAPI.Entities.Models;

namespace RoommateFinderAPI.Core
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetTags();
        Task<Tag> GetTag(Guid id);
        void Add(Tag tag);
        void Remove(Tag tag);
    }
}