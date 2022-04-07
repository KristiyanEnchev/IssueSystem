namespace IssueSystem.Services.HelpersServices.DropDown
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using IssueSystem.Data;
    using IssueSystem.Services.HelpersServices.Cache;
    using Microsoft.AspNetCore.Identity;
    using IssueSystem.Data.Models;

    public class DropDownService : IDropDownService
    {
        private readonly IssueSystemDbContext data;

        private readonly ICacheService _cache;

        private readonly RoleManager<IssueSystemRole> _roleManager;

        public DropDownService(
            IssueSystemDbContext data,
            ICacheService cache,
            RoleManager<IssueSystemRole> roleManager)
        {
            this.data = data;
            _cache = cache;
            _roleManager = roleManager;
        }

        public IEnumerable<SelectListItem> GetCategories()
        {
            var categories = _cache.Get<IEnumerable<SelectListItem>>("categories",
            () =>
            {
                return data.TicketCategories
                   .Select(c => new SelectListItem
                   {
                       Value = c.TicketCategoryId.ToString(),
                       Text = c.CategoryName
                   })
                   .ToList();
            });

            return categories;
        }

        public IEnumerable<SelectListItem> GetPriorities()
        {
            var priorities = _cache.Get<IEnumerable<SelectListItem>>("priorities",
            () =>
            {
                return data.TicketPriorities
                   .Select(c => new SelectListItem
                   {
                       Value = c.PriorityId.ToString(),
                       Text = c.PriorityType.ToString()
                   })
                   .ToList();
            });

            return priorities;
        }

        public IEnumerable<SelectListItem> GetDepartments()
        {
            var departments = _cache.Get<IEnumerable<SelectListItem>>("departments",
            () =>
            {
                return data.Departments
                   .Select(c => new SelectListItem
                   {
                       Value = c.DepartmentId.ToString(),
                       Text = c.DepartmentName,
                   })
                   .ToList();
            });

            return departments;
        }

        public IEnumerable<SelectListItem> GetRoles()
        {
            var roles = _cache.Get<IEnumerable<SelectListItem>>("roles",
            () =>
            {
                return _roleManager.Roles
                   .Select(c => new SelectListItem
                   {
                       Value = c.Id.ToString(),
                       Text = c.Name.ToString(),
                   })
                   .ToList();
            });

            return roles;
        }
    }
}
