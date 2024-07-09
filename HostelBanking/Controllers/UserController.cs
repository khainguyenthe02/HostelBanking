using HostelBanking.Entities;
using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.Enum;
using HostelBanking.Entities.Models.Account;
using HostelBanking.Services;
using HostelBanking.Services.Interfaces;
using HostelBanking.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HostelBanking.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;
        private readonly IConfiguration _config;
        public UserController(IServiceManager serviceManager, IConfiguration configuration)
		{
			this._serviceManager = serviceManager;
            _config = configuration;
        }
		private string currentEmail => HttpContext.Items["Email"]?.ToString();
		[HttpGet("get-user-by-id/{Id}")]
		public async Task<IActionResult> GetUserById(int Id)
		{
			var userDto = await _serviceManager.UserService.GetById(Id);
			if (userDto is null)
			{
				return StatusCode((int)HttpStatusCode.NoContent);
			}
			return Ok(userDto);
		}

		[HttpGet("get-users")]
		public async Task<IActionResult> GetUsers()
		{
			List<UserDto> userDto;
			userDto = await _serviceManager.UserService.GetAll();
			if (userDto == null) userDto = new();
			return Ok(userDto);
		}

		[HttpPost("create-user")]
		public async Task<IActionResult> CreateUser([FromBody] UserCreateDto createUserDto)
		{
			// Kiểm tra email có trùng hay không
			if(createUserDto.Email != null)
			{
				if (!Validate.IsValidEmail(createUserDto.Email))
				{
					return BadRequest(MessageError.TypeEmailError);
				}
				var userDto = await _serviceManager.UserService.GetByEmail(createUserDto.Email);
				if(userDto != null)
				{
					return BadRequest(MessageError.EmailExist);	
				}
				if (createUserDto.Password == null)
				{
					return BadRequest(MessageError.InvalidPasswordError);
				}
				if (!Validate.ValidatePasword(createUserDto.Password))
				{
					return BadRequest(MessageError.TypingPasswordError);
				}
				var result = await _serviceManager.UserService.Create(createUserDto);
				if (result)
				{
					return Ok();
				}
			}			
			return BadRequest(MessageError.ErrorCreate);
		}
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] AuthRequestDto loginRequest, CancellationToken cancellationToken)
		{
			loginRequest.Email = loginRequest.Email.Replace(" ", "");
			loginRequest.Password = loginRequest.Password.Replace(" ", "");

			// Kiểm tra xem người dùng có tồn tại không
			var user = await _serviceManager.UserService.GetByEmail(loginRequest.Email);
			if (user == null)
			{
				return BadRequest(MessageError.LoginError); // Email không tồn tại
			}

			// Kiểm tra trạng thái tài khoản
			if (user.StatusAccount == (int)AccountStatus.BLOCK)
			{
				return BadRequest(MessageError.AccountInActive); // Tài khoản không hoạt động
			}
			// Xác thực mật khẩu
			var userLogin = await _serviceManager.UserService.Login(loginRequest.Email, Utils.Convert.GetMD5Hash(loginRequest.Password));
			if (userLogin == null)
			{
				user.InvalidPasswordCount++;
				await _serviceManager.UserService.UpdateUser(user);

				if (user.InvalidPasswordCount >= 5)
				{
					user.StatusAccount = (int)AccountStatus.INACTIVE;
					await _serviceManager.UserService.UpdateUser(user);
					return BadRequest(MessageError.InvalidPasswordCount); // Quá số lần nhập sai cho phép
				}
				return BadRequest(MessageError.LoginError + $". Bạn đã nhập sai mật khẩu {user.InvalidPasswordCount} lần.");
			}
			// Đăng nhập thành công
			await _serviceManager.UserService.UpdateUser(user);
            var claims = new[]
               {
                new Claim("fullName",user.FullName),
                new Claim("email",user.Email),
                new Claim("phoneNumber",user.PhoneNumber),
                new Claim(ClaimTypes.Role,user.RoleName),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(30),
                signingCredentials: creds);

            var resuil = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
				infor=user
            };

            return Ok(resuil);
		}
		[HttpPost("active")]
		public async Task<IActionResult> ActiveAccount([FromBody] EmailAcountDto emailAcount, CancellationToken cancellationToken)
		{
			emailAcount.Email = emailAcount.Email.Replace(" ", "");


			// Kiểm tra xem người dùng có tồn tại không
			var user = await _serviceManager.UserService.GetByEmail(emailAcount.Email);
			if (user == null)
			{
				return BadRequest(MessageError.EmailNotExist); // Email không tồn tại
			}

			// Kiểm tra trạng thái tài khoản
			if (user.StatusAccount == (int)AccountStatus.INACTIVE)
			{
				user.StatusAccount = (int)AccountStatus.ACTIVE;
				// Đăng nhập thành công
				await _serviceManager.UserService.UpdateUser(user);
				return Ok("Kích hoạt thành công");
			}
			return StatusCode(500, " Đã xảy ra lỗi");
		

		}
		[HttpDelete]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var userDto = await _serviceManager.UserService.GetById(id);
			if (userDto != null)
			{
				await _serviceManager.UserService.Delete(id);

				return Ok(); //NoContent();
			}
			return BadRequest("Người dùng không tồn tại");
		}
		[HttpPost("update-password")]
        [Authorize]
        public async Task<IActionResult> UserUpdatePassword([FromBody] UpdatePasswordDto userUpdatePassword, CancellationToken cancellationToken)
		{

			if (!string.IsNullOrEmpty(userUpdatePassword.ReNewPassword))
			{

				//Check mail
				if (!userUpdatePassword.NewPassword.Equals(userUpdatePassword.ReNewPassword)) 
					return BadRequest(MessageError.ComparePasswordError);

				// check password invalid
				if (!Validate.ValidatePasword(userUpdatePassword.NewPassword)) 
					return BadRequest(MessageError.TypingPasswordError);
				if (!Validate.IsValidEmail(userUpdatePassword.Email))
				{
					return BadRequest(MessageError.TypeEmailError);
				}
				// check oldPass is true?
				var user = await _serviceManager.UserService.GetByEmail(userUpdatePassword.Email);
				if (user == null) return NotFound(MessageError.NoContent);
				var userLogin = await _serviceManager.UserService.Login(userUpdatePassword.Email.ToLower().Replace(" ", ""), Utils.Convert.GetMD5Hash(userUpdatePassword.OldPassword));
				if (userLogin == null) return BadRequest(MessageError.LoginError);

				// check old pass == new pass
				if (userUpdatePassword.NewPassword == userUpdatePassword.OldPassword) 
					return BadRequest(MessageError.PasswordNotEquals);
				// update password 
				if (await _serviceManager.UserService.UpdatePassword((int)user.Id, userUpdatePassword.NewPassword)) 
				{
					return Ok();
				}
				else return BadRequest(MessageError.ErrorUpdate);
			}
			else
				return BadRequest(MessageError.InvalidRePasswordError);
		}
		[HttpPut("update-user")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto updateUserDto, CancellationToken cancellationToken)
		{
			// Kiểm tra user có trùng hay không
			var userDto = await _serviceManager.UserService.GetById((int)updateUserDto.Id);
			if (userDto == null) return StatusCode((int)HttpStatusCode.BadRequest, "Người dùng không tồn tại");
			if (await _serviceManager.UserService.Update(updateUserDto))
			{
				return Ok();
			}
			return BadRequest(MessageError.ErrorUpdate);
		}

        
        [HttpPost("search-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchData([FromBody] UserSearchDto searchUserDto, CancellationToken cancellationToken)
		{
			List<UserDto> userDto;
			userDto = await _serviceManager.UserService.Search(searchUserDto);
			if (userDto == null) return Ok(new List<UserDto>());
			return Ok(userDto);

		}
	}
}
