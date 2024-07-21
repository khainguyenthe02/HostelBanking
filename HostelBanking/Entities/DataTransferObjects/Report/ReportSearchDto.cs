namespace HostelBanking.Entities.DataTransferObjects.Report
{
	public class ReportSearchDto
	{
		public int? Id { get; set; }
		public int? ReportStatus { get; set; }
		public int? PostId { get; set; }
		public int? AccountId { get; set; }
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
	}
}
