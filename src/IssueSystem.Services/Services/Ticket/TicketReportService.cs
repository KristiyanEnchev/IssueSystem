namespace IssueSystem.Services.Services.Ticket
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Models.Tickets;
    using IssueSystem.Services.Contracts.Ticket;

    public class TicketReportService : BaseService<Ticket>, ITicketReportService
    {
        public TicketReportService(
            IssueSystemDbContext data,
            IMapper mapper) 
            : base(data, mapper)
        {
        }

        public async Task<IList<UserTicketsIndexModel>> GetUserAcceptedTicketsDaily(string acceptantId)
        {
            return await Mapper.ProjectTo<UserTicketsIndexModel>
                (Data.Tickets
                .Where(x => x.AcceptantId == acceptantId)
                .Where(x => x.CreatedOn.Date == DateTime.Today))
                .ToListAsync();
        }

        public async Task<IList<UserTicketsIndexModel>> GetUserCreatedTicketsDaily(string creatorId)
        {
            return await Mapper.ProjectTo<UserTicketsIndexModel>
                (Data.Tickets
                .Where(x => x.CreatorId == creatorId)
                .Where(x => x.CreatedOn.Date == DateTime.Today))
                .ToListAsync();
        }

        public async Task<IList<UserTicketsIndexModel>> GetUserAcceptedTicketsWeekly(string acceptantId)
        {
            return await Mapper.ProjectTo<UserTicketsIndexModel>
                (Data.Tickets
                .Where(x => x.AcceptantId == acceptantId)
                .Where(x => x.CreatedOn.Date >= DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6)))
                .ToListAsync();
        }

        public async Task<IList<UserTicketsIndexModel>> GetUserCreatedTicketsWeekly(string creatorId)
        {
            return await Mapper.ProjectTo<UserTicketsIndexModel>
                (Data.Tickets
                .Where(x => x.CreatorId == creatorId)
                .Where(x => x.CreatedOn.Date >= DateTime.UtcNow.AddDays(-(int)DateTime.UtcNow.DayOfWeek - 6)))
                .ToListAsync();
        }


        public async Task<IList<UserTicketsIndexModel>> GetUserAcceptedTicketsYearly(string acceptantId)
        {
            return await Mapper.ProjectTo<UserTicketsIndexModel>
                (Data.Tickets
                .Where(x => x.AcceptantId == acceptantId)
                .Where(x => (int)x.CreatedOn.Year -1 > (int)DateTime.UtcNow.Year))
                .ToListAsync();
        }

        public async Task<IList<UserTicketsIndexModel>> GetUserCreatedTicketsYearly(string creatorId)
        {
            return await Mapper.ProjectTo<UserTicketsIndexModel>
                (Data.Tickets
                .Where(x => x.CreatorId == creatorId)
                .Where(x => (int)x.CreatedOn.Year - 1 > (int)DateTime.UtcNow.Year))
                .ToListAsync();
        }
    }
}
