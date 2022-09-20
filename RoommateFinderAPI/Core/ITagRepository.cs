using RoommateFinderAPI.Entities.Models;

namespace RoommateFinderAPI.Core
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetTags();
        Task<Tag> GetTag(int id);
        void Add(Tag client);
        void Remove(Tag client);
    }
}