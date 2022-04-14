namespace IssueSystem.Services.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

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


        public async Task DeleteAsync(object id)
        {
            TEntity entity = await GetByIdAsync(id);

            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            EntityEntry entry = this.Data.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.DbSet().Attach(entity);
            }

            entry.State = EntityState.Deleted;
        }
    }
}
