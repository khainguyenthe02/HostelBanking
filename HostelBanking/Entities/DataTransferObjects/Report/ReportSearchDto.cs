namespace HostelBanking.Entities.DataTransferObjects.Report
{
	public class ReportSearchDto
	{
		public int? Id { get; set; }
		public int? ReportStatus { get; set; }
		public int? CountReports { get; set; }
		public int? PostId { get; set; }
		public int? AccountId { get; set; }
	}
}
