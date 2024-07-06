namespace HostelBanking.Entities.DataTransferObjects.Account
{
	public class UserDto
	{
		public int? Id { get; set; }
		public string? Email { get; set; }
		public string? FullName { get; set; }
		public string? UserAddress { get; set; }
		public string? PhoneNumber { get; set; }
		public int? StatusAccount { get; set; }
		public DateTime? CreateDate { get; set; }
		public int? RoleId { get; set; }
		public string? RoleName { get; set; } // Tên quyền
		public bool? DeleteFlag { get; set; }

		public int InvalidPasswordCount { get; set; }
	}
}
