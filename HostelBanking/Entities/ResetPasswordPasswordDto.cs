using System.ComponentModel.DataAnnotations;

namespace HostelBanking.Entities
{
	public class ResetPasswordPasswordDto
	{
		public string Token { get; set; }
		[StringLength(50, MinimumLength = 8, ErrorMessage = "Password không đúng định dạng")]
		public string NewPassword { get; set; }
		[StringLength(50, MinimumLength = 8, ErrorMessage = "Password không đúng định dạng")]
		public string ReNewPassword { get; set; }
	}
}
