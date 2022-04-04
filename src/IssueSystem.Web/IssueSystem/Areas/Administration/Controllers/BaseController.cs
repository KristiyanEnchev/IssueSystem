namespace IssueSystem.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static IssueSystem.Common.IssueSystemRoles;

    [Authorize(Roles = AdministratorRoleName)]
    [Area("Administration")]
    [AutoValidateAntiforgeryToken]
    public class BaseController : Controller
    {
        /// Sets every child controller to have the administrator rights 
    }
}
