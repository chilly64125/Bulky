namespace BulkyBookWeb.Controllers.Api.Dtos
{
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int AdminCount { get; set; }
        public int CustomerCount { get; set; }
        public int EmployeeCount { get; set; }
        public int CompanyCount { get; set; }
        public int OtherRolesCount { get; set; }
    }
}
