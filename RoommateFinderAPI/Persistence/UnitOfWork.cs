using RoommateFinderAPI.Core;

namespace RoommateFinderAPI.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RoommateFinderDbContext context;

        public UnitOfWork(RoommateFinderDbContext context)
        {
            this.context = context;
        }
        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}