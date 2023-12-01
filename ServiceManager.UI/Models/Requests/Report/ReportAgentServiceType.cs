namespace ServiceManager.UI.Models.Requests.Report
{
    public class ReportAgentServiceType
    {
        public string FullName { get; set; } = null!;
        public IEnumerable<ReportAgentServiceTypeRow> ServiceTypes { get; set; } = null!;

        public int CountAll => ServiceTypes.Sum(x => x.CountAll);
        public int CountComplited => ServiceTypes.Sum(x => x.CountComplited);
        public int CountGoodComplited => ServiceTypes.Sum(x => x.CountGoodComplited);
        public int CountBadComplited => ServiceTypes.Sum(x => x.CountBadComplited);
    }

}
