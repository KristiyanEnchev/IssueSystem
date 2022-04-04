namespace IssueSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    using IssueSystem.Data.Contracts;

    public class IssueSystemRole : IdentityRole, IBaseEntity, IDeletableEntity
    {
        public IssueSystemRole()
            : this(null)
        {
        }

        public IssueSystemRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
