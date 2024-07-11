namespace HostelBanking.Entities.DataTransferObjects.PayHistory
{
    public class PayHistorySearchDto
    {
        public int? Id { get; set; }
        public string? PayCode { get; set; }
        public DateTime? PayDate { get; set; }
        public int? Type { get; set; }
        public double? Price { get; set; }
        public int? PostId { get; set; }
        public int? AccountId { get; set; }
    }
}
