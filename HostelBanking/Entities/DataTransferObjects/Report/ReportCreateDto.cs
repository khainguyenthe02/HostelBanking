namespace HostelBanking.Entities.DataTransferObjects.Report
{
	public class ReportCreateDto
	{
		public string Detail { get; set; }
		//public int? ReportStatus { get; set; }
		//public DateTime CreateDate { get; set; }
		public int PostId { get; set; }
		public int? AccountId { get; set; }
	}
}
