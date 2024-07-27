namespace HostelBanking.Entities.DataTransferObjects.Report
{
	public class ReportFeatPostTitleDto
	{
		public string? PostTitle { get; set; }
		public int? ReportStatus { get; set; }
		public int? AccountId { get; set; }
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
	}
}
