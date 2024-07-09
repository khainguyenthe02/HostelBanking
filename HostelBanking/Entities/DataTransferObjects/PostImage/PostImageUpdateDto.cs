namespace HostelBanking.Entities.DataTransferObjects.PostImage
{
	public class PostImageUpdateDto
	{
		public int Id { get; set; }
		public int PostId { get; set; }
		public string ImageUrl { get; set; }
		public bool DeleteFlag { get; set; }
	}
}
