namespace HostelBanking.Entities.DataTransferObjects.Post
{
	public class PostUpdateDto
	{
		 public int? Id { get; set; }
		public string? Title { get; set; }
		public float? Price { get; set; }
		public int? Acreage { get; set; }
		public string? District { get; set; }
		public string? Ward { get; set; }
        public string? Zalo { get; set; }
        public string? Street { get; set; }
        public string? DescriptionPost { get; set; }
		public string? Images { get; set; }
		public string? PhoneNumber { get; set; }
		public string? OwnerHouse { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public int? PaymentType { get; set; }
        public int? HostelTypeId { get; set; }
		public int? AccountId { get; set; }
		public bool? DeleteFlag { get; set; }
        public List<string> ImageList { get; set; }
    }
}
