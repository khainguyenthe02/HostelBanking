namespace HostelBanking.Entities.DataTransferObjects.Favorite
{
    public class FavoriteCreateDto
    {
        public int PostId { get; set; }
        public int AccountId { get; set; }
        public bool Status { get; set; }
        public bool DeleteFlag { get; set; }
    }
}
