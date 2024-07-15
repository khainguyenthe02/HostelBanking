namespace HostelBanking.Entities.DataTransferObjects.Favorite
{
    public class FavoriteSearchDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int AccountId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }
        public bool DeleteFlag { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
