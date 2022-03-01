namespace IssueSystem.Data.Contracts
{
    public interface IBaseEntity
    {
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
