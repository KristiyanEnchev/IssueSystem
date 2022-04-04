namespace IssueSystem.Services.Contracts
{
    using System.Linq;

    public interface IBaseService<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> GetByIdsAsync(object[] id);
        IQueryable<TEntity> All();
        IQueryable<TEntity> AllAsNoTracking();
        void Add(TEntity entity);
        int SaveChanges();
    }
}
