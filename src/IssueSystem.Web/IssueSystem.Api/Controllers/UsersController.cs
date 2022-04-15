namespace IssueSystem.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using IssueSystem.Data.Models;
    using IssueSystem.Models.Admin.User;
    using IssueSystem.Services.Api.Contracts.Users;
    using IssueSystem.Api.Infrastructures.Extensions;

    public class UsersController : ApiController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet(nameof(GetAllUsers))]
        public async Task<ActionResult<IEnumerable<UserListViewModel>>> GetAllUsers()
        {
            return await userService.GetUsers().ToActionResult();
        }

        [HttpGet(nameof(GetUserById))]
        public async Task<ActionResult<Employee>> GetUserById()
        {
            return await userService.GetUserById(this.User.GetId()).ToActionResult();
        }

        [HttpGet(nameof(GetUserForEdit))]
        public async Task<ActionResult<UserEditViewModel>> GetUserForEdit()
        {
            return await userService.GetUserForEdit(this.User.GetId()).ToActionResult();
        }

        [HttpGet(nameof(UpdateUser))]
        public async Task<ActionResult<bool>> UpdateUser(UserEditViewModel model)
        {
            return await userService.UpdateUser(model).ToActionResult();
        }
    }
}