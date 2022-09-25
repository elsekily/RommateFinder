using Microsoft.EntityFrameworkCore;
using RoommateFinderAPI.Core;
using RoommateFinderAPI.Entities.Models;

namespace RoommateFinderAPI.Persistence.Repositories
{
    public class TagsRepository : ITagRepository
    {
        private readonly RoommateFinderDbContext context;

        public TagsRepository(RoommateFinderDbContext context)
        {
            this.context = context;
        }
        public async void Add(Tag tag)
        {
            await context.Tags.AddAsync(tag);
        }

        public async Task<Tag> GetTag(Guid id)
        {
            return await context.Tags.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Tag>> GetTags()
        {
            return await context.Tags.ToListAsync();
        }
        public void Remove(Tag tag)
        {
            context.Tags.Remove(tag);
        }
    }
}