namespace HostelBanking.Entities.DataTransferObjects.Report
{
	public class ReportDto
	{
		public int? Id { get; set; }
		public string Detail { get; set; }
		public int? ReportStatus { get; set; }
		public DateTime CreateDate { get; set; }
		public int? PostId { get; set; }
		public string PostTitle { get; set; }
		public int? AccountId { get; set; }
		public string AccountName { get; set; }
		public bool? DeleteFlag { get; set; }
	}
}
