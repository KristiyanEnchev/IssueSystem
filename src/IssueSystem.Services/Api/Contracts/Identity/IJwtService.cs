namespace IssueSystem.Services.Api.Contracts.Identity
{
    using System.Threading.Tasks;

    using IssueSystem.Data.Models;
    using IssueSystem.Services.Common;

    public interface IJwtService : ITransientService
    {
        Task<string> GenerateTokenAsync(Employee user);
    }
}
