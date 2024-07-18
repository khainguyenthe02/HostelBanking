namespace HostelBanking.Entities.Models.Favorite
{
    public class Favorite : Base
    {
        public int PostId { get; set; }
        public int AccountId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
