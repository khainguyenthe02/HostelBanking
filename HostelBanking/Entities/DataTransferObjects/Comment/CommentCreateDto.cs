namespace HostelBanking.Entities.DataTransferObjects.Comment
{
    public class CommentCreateDto
    {
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public int AccountId { get; set; }
        public int PostId { get; set; }
    }
}
