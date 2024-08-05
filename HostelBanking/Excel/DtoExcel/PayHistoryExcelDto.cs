namespace HostelBanking.Excel.DtoExcel
{
	public class PayHistoryExcelDto
	{
		public string PayCode { get; set; }
		public DateTime? PayDate { get; set; }
		public string? Type { get; set; }
		public double? Price { get; set; }
		public string PostTitle { get; set; }
		public string AccountName { get; set; }
	}
}
