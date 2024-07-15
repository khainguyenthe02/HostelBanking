namespace HostelBanking.Entities.DataTransferObjects.Comment
{
    public class CommentSearchDto
    {
        public int? Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public int? AccountId { get; set; }
        public int? PostId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
