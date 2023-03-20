namespace MPD.Core.Data;

public interface IUnitOfWorkRepository
{
    public IRepository<T> GenericRepository<T>() where T : class;
    Task SaveAsync();
}