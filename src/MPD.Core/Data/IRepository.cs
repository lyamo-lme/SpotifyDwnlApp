using System.Linq.Expressions;

namespace MPD.Core.Data;

public interface IRepository<TEntity> where TEntity:class
{
    public ValueTask<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,int? take=null, int? skip=null,
        string includeProperties = "");
    public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> func, string? include=null);
    public ValueTask<TEntity> CreateAsync(TEntity model);
    public Task<bool> DeleteAsync(TEntity entity);
    public ValueTask<TEntity> UpdateAsync(TEntity model);
}