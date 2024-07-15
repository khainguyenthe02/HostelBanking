namespace HostelBanking.Entities.DataTransferObjects.Favorite
{
    public class FavoriteUpdateDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public bool DeleteFlag { get; set; }
    }
}
