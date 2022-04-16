namespace IssueSystem.Tests
{
    using AutoMapper;
    using IssueSystem.Infrastructure;

    public static class MapperMock
    {
        public static IMapper Instance
        {
            get
            {
                var profile = new MappingConfiguration();
                var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));

                return new Mapper(configuration);
            }
        }
    }
}
