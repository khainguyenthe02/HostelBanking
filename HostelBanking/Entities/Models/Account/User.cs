using System;

namespace HostelBanking.Entities.Models.Account
{
	public class User: Base
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string? FullName { get; set; }
		public string? UserAddress { get; set; }
		public string? PhoneNumber { get; set; }
		public int? StatusAccount { get; set; }
		public DateTime? CreateDate { get; set; }
		public int? RoleId { get; set; }

		public int InvalidPasswordCount { get; set; }
	}
}
