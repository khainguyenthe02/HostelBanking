namespace HostelBanking.Entities.Models.Report
{
    public class Report : Base
    {
        public string Detail { get; set; }
        public int ReportStatus { get; set; }
        public int CountReports { get; set; }
        public DateTime CreateDate { get; set; }
        public int PostId { get; set; }
        public int AccountId { get; set; }
    }
}
