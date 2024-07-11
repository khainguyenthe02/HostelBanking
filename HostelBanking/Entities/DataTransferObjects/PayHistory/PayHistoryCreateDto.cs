namespace HostelBanking.Entities.DataTransferObjects.PayHistory
{
    public class PayHistoryCreateDto
    {
        public int PostId { get; set; }
        public int AccountId { get; set; }
        public string PayCode { get; set; }
        public DateTime PayDate { get; set; }
        public int Type { get; set; }
        public double Price { get; set; }
    }
}
