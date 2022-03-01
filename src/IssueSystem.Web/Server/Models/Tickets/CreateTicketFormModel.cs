namespace Server.Models.Tickets
{
    public class CreateTicketFormModel
    {

        public string Title { get; init; }
        public string TicketCreator { get; init; }

        public string TicketCategory { get; init; }

        public string TicketPriority { get; init; }

        public string CurrentStatus { get; init; }

        public byte[] Photo { get; init; }

        public string Description { get; init; }

        public string Comment { get; init; }
    }
}
