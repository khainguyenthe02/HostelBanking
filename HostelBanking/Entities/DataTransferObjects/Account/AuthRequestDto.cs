using System.ComponentModel.DataAnnotations;

namespace HostelBanking.Entities.DataTransferObjects.Account
{
	public class AuthRequestDto
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		public bool IsKeepLogin { get; set; } = false;
	}
}
