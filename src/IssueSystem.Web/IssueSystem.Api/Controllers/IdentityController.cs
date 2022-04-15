namespace IssueSystem.Api.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;

    using IssueSystem.Models.Api.Identity;
    using IssueSystem.Api.Infrastructures.Extensions;
    using IssueSystem.Services.Api.Contracts.Identity;

    public class IdentityController : ApiController
    {
        private readonly IIdentityService identity;

        public IdentityController(IIdentityService identity)
        {
            this.identity = identity;
        }
        [HttpPost(nameof(Register))]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            return await identity.RegisterAsync(model).ToActionResult();
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<LoginReturnModel>> Login(LoginModel model)
        {
            var result = await identity.LoginAsync(model);

            if (result.result.Succeeded)
            {
                return await Token(result.token).ToActionResult();
            }
            else 
            {
                return await Error(result.result).ToActionResult();
            }
        }

        private async Task<LoginReturnModel> Token(LoginReturnModel token) 
        {
            return token;
        }

        private async Task<IdentityResult> Error(IdentityResult errorResult)
        {
            return errorResult;
        }

        [Authorize]
        [HttpPut(nameof(ChangeSettings))]
        public async Task<ActionResult> ChangeSettings(ChangeSettingsModel model)
        {
            return await identity.ChangeSettingsAsync(model, this.User.GetId()).ToActionResult();
        }

        [Authorize]
        [HttpPut(nameof(ChangePassword))]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            return await identity.ChangePasswordAsync(model, this.User.GetId()).ToActionResult();
        }
    }
}
