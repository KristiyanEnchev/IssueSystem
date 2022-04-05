namespace IssueSystem.Services.HelpersServices.DropDown
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using IssueSystem.Data;
    using IssueSystem.Services.HelpersServices.Cache;

    public class DropDownService : IDropDownService
    {
        private readonly IssueSystemDbContext data;
        private readonly ICacheService _cache;

        public DropDownService(IssueSystemDbContext data, ICacheService cache)
        {
            this.data = data;
            _cache = cache;
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
            var categories = _cache.Get<IEnumerable<SelectListItem>>("priorities",
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

            return categories;
        }
    }
}
