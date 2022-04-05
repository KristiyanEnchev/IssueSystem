namespace IssueSystem.Services.HelpersServices.DropDown
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IDropDownService
    {
        IEnumerable<SelectListItem> GetCategories();
        IEnumerable<SelectListItem> GetPriorities();
    }
}
