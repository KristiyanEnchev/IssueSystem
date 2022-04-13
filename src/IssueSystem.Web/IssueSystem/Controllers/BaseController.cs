namespace IssueSystem.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class BaseController : Controller
    {
    }
}
