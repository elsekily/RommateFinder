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
        public void Add(Tag client)
        {
            throw new NotImplementedException();
        }

        public async Task<Tag> GetTag(int id)
        {
            return await context.Tags.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Tag>> GetTags()
        {
            var x = context.Tags;
            return await context.Tags.ToListAsync();
        }
        public void Remove(Tag client)
        {
            context.Remove(client);
        }
    }
}