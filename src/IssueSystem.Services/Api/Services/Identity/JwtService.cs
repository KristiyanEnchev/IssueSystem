namespace IssueSystem.Services.Api.Services.Identity
{
    using System.Text;
    using System.Security.Claims;
    using System.IdentityModel.Tokens.Jwt;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    using IssueSystem.Data.Models;
    using IssueSystem.Models;
    using IssueSystem.Services.Api.Contracts.Identity;

    public class JwtService : IJwtService
    {
        private readonly UserManager<Employee> userManager;
        private readonly TokenSettings tokenSettings;

        public JwtService(UserManager<Employee> UserManager, IOptions<TokenSettings> TokenSettings)
        {
            userManager = UserManager;
            tokenSettings = TokenSettings.Value;
        }

        public async Task<string> GenerateTokenAsync(Employee user)
        {
            var claimTypeFirstName = "FirstName";
            var claimType = "Surname";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(claimTypeFirstName, user.FirstName),
                new Claim(claimType, user.LastName)
            };

            var isAdministrator = await userManager.IsInRoleAsync(user, IssueSystemRoles.AdministratorRoleName);

            if (isAdministrator)
            {
                claims.Add(new Claim
                    (
                        ClaimTypes.Role,
                        IssueSystemRoles.AdministratorRoleName
                    ));
            }

            var secret = Encoding.UTF8.GetBytes(tokenSettings.Secret);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256));

            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
    }
}
