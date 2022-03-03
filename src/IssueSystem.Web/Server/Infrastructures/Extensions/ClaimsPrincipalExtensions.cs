namespace Server.Infrastructures.Extensions
{
    using System.Security.Claims;

    using static Areas.Admin.AdminConstants;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static string GetEmail(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.Email);

        public static string GetFirstName(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.Name);

        public static string GetLastName(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.Surname);

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRoleName);
    }
}
