namespace HostelBanking.Entities.DataTransferObjects.Favorite
{
    public class FavoriteUpdateDto
    {
        //public int Id { get; set; }
        public int? PostId { get; set; }
        public int? AccountId { get; set; }
        public bool DeleteFlag { get; set; }
    }
}
