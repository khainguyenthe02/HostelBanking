namespace HostelBanking.Entities.Models.Comment
{
    public class Comment : Base
    {
        public string Content {  get; set; }
        public DateTime CreateDate { get; set; }
        public int AccountId { get; set; }
        public int PostId { get; set; }
    }
}
