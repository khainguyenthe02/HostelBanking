using System.ComponentModel.DataAnnotations;

namespace HostelBanking.Entities.DataTransferObjects.Account
{
    public class forgotPassDto
    {
        public int? Id { get; set; }
        public string NewPassword { get; set; }
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu không được quá 50 ký tự và phải lớn hơn 6 ký tự")]
        public string ReNewPassword { get; set; }
    }
}
