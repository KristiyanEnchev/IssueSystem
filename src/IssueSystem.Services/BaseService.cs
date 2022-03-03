namespace IssueSystem.Services
{
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    using IssueSystem.Data;

    public class BaseService<TEntity>
        where TEntity : class
    {
        protected BaseService(IssueSystemDbContext data, IMapper mapper)
        {
            this.Data = data;
            this.Mapper = mapper;
        }

        protected IssueSystemDbContext Data { get; }

        protected IMapper Mapper { get; }

        protected IQueryable<TEntity> All() => this.Data.Set<TEntity>();

        protected IQueryable<TEntity> AllAsNoTracking() => this.All().AsNoTracking();
    }
}
