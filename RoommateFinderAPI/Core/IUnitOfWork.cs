namespace RoommateFinderAPI.Core
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}