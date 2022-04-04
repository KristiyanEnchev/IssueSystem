namespace IssueSystem.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    using IssueSystem.Data;
    using IssueSystem.Services.Contracts;

    public abstract class BaseService<TEntity> : IBaseService<TEntity>
        where TEntity : class
    {
        protected BaseService(IssueSystemDbContext data, IMapper mapper)
        {
            this.Data = data;
            this.Mapper = mapper;
        }

        protected IssueSystemDbContext Data { get; }
        protected IMapper Mapper { get; }
        protected DbSet<TEntity> DbSet() => Data.Set<TEntity>();

        public async Task<TEntity> GetByIdAsync(object id) => await Data.Set<TEntity>().FindAsync(id);

        public async Task<TEntity> GetByIdsAsync(object[] id) => await Data.Set<TEntity>().FindAsync(id);

        public void Add(TEntity entity) => DbSet().Add(entity);

        public IQueryable<TEntity> All() => Data.Set<TEntity>();

        public IQueryable<TEntity> AllAsNoTracking() => All().AsNoTracking();

        public int SaveChanges() => Data.SaveChanges();
    }
}
