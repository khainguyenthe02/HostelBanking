using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.Roles;
using HostelBanking.Entities.Models.Account;

namespace HostelBanking.Services.Interfaces
{
	public interface IUserService
	{
		Task<bool> Create(UserCreateDto user);
		Task<List<UserDto>> GetAll();
		Task<bool> Delete(int id);
		Task<UserDto> GetById(int id);
		Task<bool> Update(UserUpdateDto user);
		Task<bool> UpdateUser(UserDto userDto);
		Task<List<UserDto>> Search(UserSearchDto search);
		Task<User> Login(string email, string password);
		Task<UserDto> Login(AuthRequestDto loginRequest);
		Task<UserDto> GetByEmail(string email);
		Task<bool> UpdatePassword(int Id, string newPassword);
	}
}
