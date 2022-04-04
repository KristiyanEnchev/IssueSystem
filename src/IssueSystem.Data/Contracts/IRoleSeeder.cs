namespace IssueSystem.Data.Contracts
{
    public interface IRoleSeeder
    {
        bool CreateManagerRole();
        bool CreateEmployeeRole();
        bool CreateAdministratorRole();
    }
}
