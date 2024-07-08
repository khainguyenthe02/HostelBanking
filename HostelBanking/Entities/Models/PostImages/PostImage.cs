namespace HostelBanking.Entities.Models.PostImages
{
	public class PostImage: Base
	{
		public int PostId { get; set; }
		public string? ImageName { get; set; }
		public string? ImageUrl { get; set;	 }
	}
}
