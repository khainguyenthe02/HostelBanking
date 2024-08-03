using Firebase.Auth;
using HostelBanking.Entities;
using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects;
using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.Post;
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
        private readonly IDiscountService _discountService;
        public UserController(IServiceManager serviceManager, IConfiguration configuration, IDiscountService discountService)
		{
			this._serviceManager = serviceManager;
            _config = configuration;
            _discountService= discountService;
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

        [HttpGet("get-user-by-email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var userDto = await _serviceManager.UserService.GetByEmail(email);
            if (userDto is null)
            {
                return StatusCode((int)HttpStatusCode.NoContent);
            }
            return Ok(userDto);
        }

        [HttpGet("get-users")]
		[Authorize(Roles = "Admin")]
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
			user.InvalidPasswordCount = 0;
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


            var priceForPayment = new PriceForPaymentDto();
            var discount = await _discountService.LoadFromFile();
            var postSearch = new PostSearchDto
            {
                AccountId = user.Id
            };
            var listPost = await _serviceManager.PostService.Search(postSearch);
            if (listPost == null || listPost.Count == 0)
            {
                priceForPayment.CreatedPrice = discount.CreatedPrice;
                priceForPayment.UpdatedPrice = discount.UpdatedPrice;
            }
            else
            {
                var multiple = Math.Floor((float)listPost.Count / discount.CountPostToSale); // 50 : 10 = 5

                if (multiple >= 1)
                {
                    multiple = multiple * (discount.PercentSale / 100f);// 5*0.1 = 0.5
                    if (multiple > 0.5)
                    {
                        multiple = 0.5f;
                    }
                }
                priceForPayment.CreatedPrice = (float)(discount.CreatedPrice - discount.CreatedPrice * multiple);// 15 = 30 - 30*0.5
                priceForPayment.UpdatedPrice = (float)(discount.UpdatedPrice - discount.UpdatedPrice * multiple);
            }
            var resuil = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
				infor=user,
                priceForPayment
            };
            return Ok(resuil);
		}
		[HttpPost("active")]
		[Authorize(Roles = "Admin")]
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
			if (user.StatusAccount == (int)AccountStatus.NOACTIVE)
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



        [HttpPost("forgot-password")]
        public async Task<IActionResult> UserForgotPassword([FromBody] ForgotPasswordDto userforgotPassword, CancellationToken cancellationToken)
        {

            if (!string.IsNullOrEmpty(userforgotPassword.ReNewPassword))
            {

                //Check mail
                if (!userforgotPassword.NewPassword.Equals(userforgotPassword.ReNewPassword))
                    return BadRequest(MessageError.ComparePasswordError);

                // check password invalid
                if (!Validate.ValidatePasword(userforgotPassword.NewPassword))
                    return BadRequest(MessageError.TypingPasswordError);
               
                // check oldPass is true?
                var user = await _serviceManager.UserService.GetById((int)userforgotPassword.Id);
               
                // update password 
                if (await _serviceManager.UserService.UpdatePassword((int)user.Id, userforgotPassword.NewPassword))
                {
                    return Ok();
                }
                else return BadRequest(MessageError.ErrorUpdate);
            }
            else
                return BadRequest(MessageError.InvalidRePasswordError);
        }



        [HttpPut("update-user")]
        //[Authorize]
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
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchData([FromBody] UserSearchDto searchUserDto, CancellationToken cancellationToken)
		{
			List<UserDto> result = new();
			result = await _serviceManager.UserService.Search(searchUserDto);
			var count = result.Count();
			if (count > 0)
			{
				var pageIndex = searchUserDto.PageNumber;
				int pageSize = (int)searchUserDto.PageSize;
				var numberPage = Math.Ceiling((float)(count / pageSize));
				int start = (pageIndex - 1) * pageSize;
				var post = result.Skip(start).Take(pageSize);
				return Ok(new
				{
					data = post,
					totalItem = result.Count,
					numberPage,
					searchUserDto.PageNumber,
					searchUserDto.PageSize
				});
			}
			return Ok(new List<UserDto>());

		}

        [HttpPost("search-user-by-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchDataByAdmin([FromBody] UserSearchDto searchUserDto, CancellationToken cancellationToken)
        {
            List<UserDto> userDto;
            userDto = await _serviceManager.UserService.SearchByAdmin(searchUserDto);
			var count= userDto.Count();
			if (count > 0)
			{
                var pageIndex = searchUserDto.PageNumber;
                int pageSize = (int)searchUserDto.PageSize;
                var numberPage = Math.Ceiling((float)(count / pageSize));
                int start = (pageIndex - 1) * pageSize;
                var post = userDto.Skip(start).Take(pageSize);
                return Ok(new
                {
                    data = post,
                    totalItem = userDto.Count,
                    numberPage,
                    searchUserDto.PageNumber,
                    searchUserDto.PageSize
                });
            }
          return Ok(new List<UserDto>());
            
        }
    }
}
