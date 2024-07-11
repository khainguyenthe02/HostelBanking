namespace HostelBanking.Entities.DataTransferObjects.PayHistory
{
    public class PayHistoryUpdateDto
    {
        public int? Id { get; set; }
        public string PayCode { get; set; }
        public DateTime PayDate { get; set; }
        public int? Type { get; set; }
        public double? Price { get; set; }
        public bool? DeleteFlag { get; set; }
        public int? PostId { get; set; }
        public int? AccountId { get; set; }
    }
}
