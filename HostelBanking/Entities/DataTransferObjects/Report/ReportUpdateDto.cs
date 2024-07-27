namespace HostelBanking.Entities.DataTransferObjects.Report
{
	public class ReportUpdateDto
	{
		public int? Id { get; set; }
		public int? ReportStatus { get; set; }
		public bool? DeleteFlag { get; set; }
	}
}
