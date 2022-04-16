namespace IssueSystem.Tests.Services.Admin
{
    using System.Threading.Tasks;

    using Xunit;
    using Shouldly;

    using IssueSystem.Services.Admin.Services;
    using IssueSystem.Tests.Common;
    using IssueSystem.Tests.Data;
    using IssueSystem.Models.Admin.Ticket;

    public class AdminTicketSetviceTest : SetupFixture
    {
        private AdminTicketService _adminTicketService;
        public AdminTicketSetviceTest()
        {
            this._adminTicketService = new AdminTicketService
            (
                this.Data,
                this.Mapper
            );
        }

        [Fact]
        public async Task GetTicketInfo_Shoild_work_correctly() 
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);
            await this.AddFakeTickets(1);

            var model = await _adminTicketService.GetTicketsInfo();

            Assert.IsType<TicketIndexModel>(model[0]);
            model.ShouldNotBeNull();
            model.Count.ShouldBeGreaterThan(0);
            model[0].ShouldNotBeNull();
        }

        [Fact]
        public async Task GetTicketDaylyInfo_Shoild_work_correctly()
        {
            await this.AddFakeDepartments(1);
            await this.AddFakeProjects(1);
            await this.AddFakeEmployees(1);
            await this.AddFakeTickets(1);

            var model = await _adminTicketService.GetTicketsdailyInfo();

            Assert.IsType<TicketIndexModel>(model[0]);
            model.ShouldNotBeNull();
            model.Count.ShouldBeGreaterThan(0);
            model[0].ShouldNotBeNull();
        }

        public async Task AddFakeDepartments(int count)
        {
            var fakes = DepartmentsTestData.Getdepartments(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }
        public async Task AddFakeTickets(int count)
        {
            var fakes = TicketsTestData.GetTicketsNoAccepted(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }

        public async Task AddFakeEmployees(int count)
        {
            var fakes = UsersTestData.GetEmployeesForDepartment(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }

        public async Task AddFakeProjects(int count)
        {
            var fakes = ProjectsTestData.GetProjects(count);

            await this.Data.AddRangeAsync(fakes);
            await this.Data.SaveChangesAsync();
        }
    }
}
