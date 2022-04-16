namespace IssueSystem.Tests.Common
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public class ClaimsPrincipleMoq
    {
        public static ClaimsPrincipal Instance(string userId = "TestId")
        {
            var fakeClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            };

            var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");

            var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);

            return fakeClaimsPrincipal;
        }
    }
}
