namespace VirtoCommerce.Platform.Web.Infrastructure.HealthCheck
{
    public record struct ModulesHealthReportRecord
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string[] Errors { get; set; }
    }
}
