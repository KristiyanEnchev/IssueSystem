namespace IssueSystem.Data.Contracts
{
    public class BaseDeletableEntity : IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
