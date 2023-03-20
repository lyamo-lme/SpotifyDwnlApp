using Microsoft.Extensions.Logging;
using MPD.Core.Data;
using MPD.Data.DbContext;

namespace MPD.Data.Repository;

public class UowRepository : IUnitOfWorkRepository, IDisposable
{
    private readonly ILogger<UowRepository>? _logger;
    private readonly AppDbContext _context;
    private readonly IRepositoryFactory _repositoryFactory;
    private Dictionary<string, object>? _repositories;

    public UowRepository(ILogger<UowRepository> logger, IRepositoryFactory repositoryFactory, AppDbContext context)
    {
        _logger = logger;
        _repositoryFactory = repositoryFactory;
        _context = context;
    }

    public UowRepository(IRepositoryFactory repositoryFactory,
        AppDbContext context)
    {
        _repositoryFactory = repositoryFactory;
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IRepository<T> GenericRepository<T>() where T : class
    {
        if (_repositories == null)
        {
            _repositories = new Dictionary<string, object>();
        }

        var type = typeof(T).Name;
        if (!_repositories.ContainsKey(type))
        {
            var repository = _repositoryFactory.Instance<T>(_context);
            _repositories.Add(type, repository);
        }

        return (IRepository<T>)_repositories[type];
    }

    public Task SaveAsync()
    {
        try
        {
            return _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Critical, e, e.Message);
            throw;
        }
    }
}