namespace ServiceManager.UI.Models.Requests.Report
{
    public class ReportAgentServiceTypeRow
    {
        public string Name { get; set; } = null!;

        public int CountAll { get; set; }
        public int CountComplited => CountGoodComplited + CountBadComplited;
        public int CountGoodComplited { get; set; }
        public int CountBadComplited { get; set; }
    }

}
