namespace HostelBanking.Entities.DataTransferObjects.PayHistory
{
	public class PayHistoryFeatPostTitleDto
	{
		public string? PostTitle { get; set; }
		public int? Type { get; set; }
		public int?  AccountId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
