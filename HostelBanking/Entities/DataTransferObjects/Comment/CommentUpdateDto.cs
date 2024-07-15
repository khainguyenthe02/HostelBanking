namespace HostelBanking.Entities.DataTransferObjects.Comment
{
    public class CommentUpdateDto
    {
        public int? Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public int? AccountId { get; set; }
        public int? PostId { get; set; }
        public bool? DeleteFlag { get; set; }
    }
}
