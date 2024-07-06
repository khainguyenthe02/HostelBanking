namespace HostelBanking.Entities.DataTransferObjects.Account
{
	public class UserUpdateDto
	{
		public int? Id { get; set; }
		public string? FullName { get; set; }
		public string? UserAddress { get; set; }
		public string? PhoneNumber { get; set; }
		public int? StatusAccount { get; set; }
		public int? RoleId { get; set; }
		public bool? DeleteFlag { get; set; }
	}
}
