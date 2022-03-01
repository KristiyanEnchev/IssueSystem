namespace IssueSystem.Data.Contracts
{
    public class BaseEntity : IBaseEntity
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
