using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.Roles;
using HostelBanking.Entities.Enum;
using HostelBanking.Entities.Models.Account;
using HostelBanking.Repositories;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using HostelBanking.Utils;
using Mapster;
using Serilog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HostelBanking.Services
{
	public class UserService : IUserService
	{
		private readonly IRepositoryManager _repositoryManager;
		public UserService(IRepositoryManager repositoryManager)
		{
			this._repositoryManager = repositoryManager;
		}

		public async Task<bool> Create(UserCreateDto createUserDto)
		{
			var user = createUserDto.Adapt<User>();

			user.CreateDate = DateTime.UtcNow;
			user.Password = Utils.Convert.GetMD5Hash(user.Password);
			user.Email = user.Email.ToLower().Replace(" ", "");
			user.DeleteFlag = false;

			// Kiểm tra role
			if(user.RoleId == null) {  return false; }
			var roleId = (int) user.RoleId;
			var role = await _repositoryManager.RoleRepository.GetById(roleId);
			if(role == null)
			{
				return false;
			}
			if (user.StatusAccount == null)
			{
				user.StatusAccount = (int)AccountStatus.ACTIVE;
			}
			var result = await _repositoryManager.UserRepository.Create(user);
			if (result)
			{
				return true;
			}
			return false;

		}

		public async Task<bool> Delete(int id)
		{
			UserSearchDto search = new()
			{
				Id = id,
			};
			var userInfo = await _repositoryManager.UserRepository.Search(search);
			if (userInfo != null)
			{
				var userUpdate = new User
				{
					Id = id,
					DeleteFlag = true
				};
				var result = await _repositoryManager.UserRepository.Update(userUpdate);
				return result;
			}
			return false;
		}

		public async Task<List<UserDto>> GetAll()
		{
			var result = await _repositoryManager.UserRepository.GetAll();
			return result?.Adapt<List<UserDto>>();
		}

		public async Task<UserDto> GetByEmail(string email)
		{
			UserSearchDto search = new();
			search.Email = email;
			var user = (await _repositoryManager.UserRepository.Search(search))?.FirstOrDefault();

			var userDto = user?.Adapt<UserDto>();
			if (userDto != null)
			{
				return ( await FilterData(new() { userDto })).FirstOrDefault();
			}
			return null;
		}

		public async Task<UserDto> GetById(int id)
		{
			var result = await _repositoryManager.UserRepository.GetById(id);
			return result?.Adapt<UserDto>();
		}

		public async Task<User> Login(string email, string password)
		{
			var existingUser = await _repositoryManager.UserRepository.Login(email, password);
			if (existingUser != null)
			{
				//đăng nhập thành công => InvalidPasswordCount = 0
				existingUser.InvalidPasswordCount = 0;
				var result =  await _repositoryManager.UserRepository.Update(existingUser);
				return existingUser;
			}
			return null;
			
		}

		public async Task<List<UserDto>> Search(UserSearchDto search)
		{
			var result = await _repositoryManager.UserRepository.Search(search);
			var resultDto = result.Adapt<List<UserDto>>();
			return await FilterData(resultDto);
		}

		public async Task<bool> Update(UserUpdateDto userForUpdateDto)
		{
			UserSearchDto search = new();
			search.Id = userForUpdateDto.Id;
			var existingUser = (await _repositoryManager.UserRepository.Search(search))?.FirstOrDefault();
			if (existingUser == null) return false;
			var user = userForUpdateDto.Adapt<User>();
			// Không update pass
			if (user.Password != null)
			{
				user.Password = existingUser.Password;
			}
			var result = await _repositoryManager.UserRepository.Update(user);
			if (result)
			{
				return true;
			}			
			return false;
		}

		public async Task<bool> UpdatePassword(int id, string newPassword)
		{
			var existingUser = await _repositoryManager.UserRepository.GetById(id);
			if (existingUser != null)
			{
				existingUser.Password = Utils.Convert.GetMD5Hash(newPassword);
				var result = await _repositoryManager.UserRepository.Update(existingUser);
				if (result)
				{
					return true;
				}
			}
			return false;
		}
		public async Task<List<UserDto>> FilterData(List<UserDto> lstUsers)
		{
			if(lstUsers?.Count > 0)
			{
				var roleIdLst = lstUsers.Where(x => x.RoleId.HasValue).Select(x => x.RoleId.GetValueOrDefault()).ToList();
				if (roleIdLst.Count > 0)
				{
					var searchRole = new RoleSearchDto()
					{
						IdLst = roleIdLst
					};
					var roles = (await _repositoryManager.RoleRepository.Search(searchRole))?.ToDictionary(x => x.Id, x => x.RoleName);
					if(roles?.Count > 0)
					{
						foreach (var item in lstUsers)
						{
							if(item.RoleId.HasValue && roles.ContainsKey(item.RoleId.Value))
							{
								item.RoleName = roles[item.RoleId.Value];
							}
						}
					}
				}
			}
			return lstUsers;
		}

		public async Task<bool> UpdateUser(UserDto userDto)
		{
			var user = userDto.Adapt<User>();
			var result = await _repositoryManager.UserRepository.Update(user);
			if (result)
			{
				return true;
			}
			return false;
		}

		public Task<UserDto> Login(AuthRequestDto loginRequest)
		{
			throw new NotImplementedException();
		}
	}
}
