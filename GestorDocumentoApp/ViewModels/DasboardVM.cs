namespace GestorDocumentoApp.ViewModels
{
    public class DasboardVM
    {
        public List<ChangeRequestSummaryVM> ChangeRequestSummary { get; set; } = new();
        public List<ProjectConfigurationVM> ProjectConfigurations { get; set; } = new();
    }

    public class ChangeRequestSummaryVM
    {
        public string ProjectName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Total { get; set; }
    }

    public class ProjectConfigurationVM
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ElementName { get; set; } = string.Empty;
        public string? LatestVersion { get; set; }
        public DateTime? VersionDate { get; set; }
        public string? Status { get; set; }
    }
}
