﻿using System.ComponentModel.DataAnnotations;

namespace HostelBanking.Entities
{
	public class UpdatePasswordDto
	{
		public string Email { get; set; }
		public string OldPassword { get; set; }
		[StringLength(50, MinimumLength = 8, ErrorMessage = "Độ dài mật khẩu không được quá 50 ký tự và phải lớn hơn 8 ký tự")]
		public string NewPassword { get; set; }
		[StringLength(50, MinimumLength = 8, ErrorMessage = "Độ dài mật khẩu không được quá 50 ký tự và phải lớn hơn 8 ký tự")]
		public string ReNewPassword { get; set; }
	}
}
