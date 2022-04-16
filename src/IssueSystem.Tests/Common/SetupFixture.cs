namespace IssueSystem.Tests.Common
{
    using System;

    using AutoMapper;

    using IssueSystem.Data;
    using IssueSystem.Infrastructure;
    using Microsoft.EntityFrameworkCore;

    public class SetupFixture : IDisposable
    {
        protected SetupFixture()
        {
            this.Data = InitializeDbContext();
            this.Mapper = InitializeAutoMapper();
        }

        protected IssueSystemDbContext Data { get; }

        protected IMapper Mapper { get; }

        public void Dispose() => this.Data?.Dispose();

        private static IssueSystemDbContext InitializeDbContext()
        {
            var options = new DbContextOptionsBuilder<IssueSystemDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new IssueSystemDbContext(options);
        }

        private static IMapper InitializeAutoMapper()
        {
            var profile = new MappingConfiguration();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));

            return new Mapper(configuration);
        }
    }
}
