namespace HostelBanking.Entities.DataTransferObjects.Comment
{
    public class CommentDto
    {
        public int? Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
        public int? PostId { get; set; }
        public string PostTitle { get; set; }
        public bool? DeleteFlag { get; set; }
    }
}
