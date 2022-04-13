namespace IssueSystem.Services.Services.Ticket
{
    using System.Threading.Tasks;

    using AutoMapper;

    using IssueSystem.Data;
    using IssueSystem.Data.Models;
    using IssueSystem.Services.Contracts.Ticket;
    using IssueSystem.Data.Models.Enumeration;
    using Microsoft.EntityFrameworkCore;

    public class StatusService : BaseService<TicketStatus>, IStatusService
    {
        public StatusService(
            IssueSystemDbContext data,
            IMapper mapper)
            : base(data, mapper)
        {
        }

        public async Task<(bool acceped, TicketStatus status)> Accept(string acceptantId, string ticketId)
        {
            var accepted = false;

            var acceptedStatus = new TicketStatus();

            var ticket = await Data.Tickets.FirstOrDefaultAsync(x => x.TicketId == ticketId);
            var currentstatus = ticket.TicketStatuses
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefault()
                .StatusType;

            ///not sure if creator can accept ticket if no 
            ///&& ticket.AcceptantId != acceptantId

            if (acceptantId != null &&
                ticketId != null &&
                ticket != null &&
                currentstatus != StatusType.Accepted)
            {
                acceptedStatus.TicketId = ticketId;
                acceptedStatus.EmployeeId = acceptantId;
                acceptedStatus.StatusType = StatusType.Accepted;

                await Data.TicketStatuses.AddAsync(acceptedStatus);

                ticket.AcceptantId = acceptantId;

                await Data.SaveChangesAsync();

                accepted = true;
            }

            return (accepted, acceptedStatus);
        }

        public async Task<(bool closed, TicketStatus status)> Close(string creatorId, string ticketId)
        {
            var closed = false;

            var closedStatus = new TicketStatus();

            if (creatorId != null && ticketId != null)
            {
                closedStatus.TicketId = ticketId;
                closedStatus.EmployeeId = creatorId;
                closedStatus.StatusType = StatusType.Closed;

                await Data.TicketStatuses.AddAsync(closedStatus);

                await Data.SaveChangesAsync();

                closed = true;
            }

            return (closed, closedStatus);
        }

        public async Task<(bool opened, TicketStatus status)> Open(string creatorId, string ticketId)
        {
            var opened = false;

            var openStatus = new TicketStatus();

            if (creatorId != null && ticketId != null)
            {
                openStatus.TicketId = ticketId;
                openStatus.EmployeeId = creatorId;
                openStatus.StatusType = StatusType.Open;

                await Data.TicketStatuses.AddAsync(openStatus);

                await Data.SaveChangesAsync();

                opened = true;
            }

            return (opened, openStatus);
        }
    }
}
