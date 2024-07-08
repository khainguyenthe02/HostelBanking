namespace HostelBanking.Entities.DataTransferObjects.PostImage
{
	public class PostImageDto
	{
		public int Id { get; set; }
		public int PostId { get; set; }
		public string ImageName { get; set; }
		public string ImageUrl { get; set; }
		public bool DeleteFlag { get; set; }
	}
}
