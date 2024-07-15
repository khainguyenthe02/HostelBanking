namespace HostelBanking.Entities.DataTransferObjects.Favorite
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public string PostTitle { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }
        public bool DeleteFlag { get; set; }

    }
}
