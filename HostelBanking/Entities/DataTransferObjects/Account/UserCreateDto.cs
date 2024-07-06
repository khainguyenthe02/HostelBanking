namespace HostelBanking.Entities.DataTransferObjects.Account
{
	public class UserCreateDto
	{
		public string? Email { get; set; }
		public string? Password { get; set; }
		public string? FullName { get; set; }
		public string? UserAddress { get; set; }
		public string? PhoneNumber { get; set; }
		public int? StatusAccount { get; set; }
		public int? RoleId { get; set; }
	}
}
