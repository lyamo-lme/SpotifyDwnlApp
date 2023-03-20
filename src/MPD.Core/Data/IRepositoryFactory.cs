namespace MPD.Core.Data;

public interface IRepositoryFactory
{
    public IRepository<T> Instance<T>(object dbContext) where T:class;
}