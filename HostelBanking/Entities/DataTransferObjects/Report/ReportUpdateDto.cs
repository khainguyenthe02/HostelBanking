namespace HostelBanking.Entities.DataTransferObjects.Report
{
	public class ReportUpdateDto
	{
		public int? Id { get; set; }
		public List<string> Detail { get; set; }
		public int? ReportStatus { get; set; }
		public int? CountReports { get; set; }
		public int? PostId { get; set; }
		public int? AccountId { get; set; }
		public bool? DeleteFlag { get; set; }
	}
}
