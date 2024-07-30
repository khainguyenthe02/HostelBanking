using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.Models.Account;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.SqlServerDbHelper.Interfaces;
using HostelBanking.SqlServerDbHelper;

namespace HostelBanking.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly IDbService _dbService;

		public UserRepository(IConfiguration configuration)
		{
			_dbService = new DbService(configuration);
		}
		public async Task<bool> Create(User user)
		{
			var result = await _dbService.EditData(
			"INSERT INTO account ( email, password, full_name, user_address, phone_number, status_account, create_date, role_id, invalid_password_count, delete_flag) " +
			"VALUES ( @Email, @Password, @FullName, @UserAddress, @PhoneNumber, @StatusAccount, @CreateDate, @RoleId,@InvalidPasswordCount, @DeleteFlag)",
			user);
			if (result > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> Delete(int id)
		{
			var result = await _dbService.EditData("DELETE FROM account WHERE id = @Id", new { Id = id });
			return true;
		}

		public async Task<List<User>> GetAll()
		{
			var userList = await _dbService.GetAll<User>("SELECT * FROM account", new { });
			return userList;
		}

		public async Task<User> GetById(int id)
		{
			var user = await _dbService.GetAsync<User>("SELECT * FROM account WHERE id = @Id", new { Id = id });
			return user;
		}

		public async Task<User> Login(string email, string password)
		{
			User user = new();
			user.Email = email;
			user.Password = password;
			var selectSql = $" SELECT * FROM account ";

			var whereSql = " where delete_flag = 0 ";
			if (!string.IsNullOrEmpty(email))
			{
				whereSql += $" and email=@Email ";
			}
			if (!string.IsNullOrEmpty(password))
			{
				whereSql += $" and password=@Password ";
			}
			var result = await _dbService.GetAll<User>(selectSql + whereSql, user);
			return result?.FirstOrDefault();
		}

		public async Task<List<User>> Search(UserSearchDto search)
		{
			var selectSql = "SELECT * FROM account ";
			var whereSql = " WHERE delete_flag  = 0 ";

			if (search.Id != null)
			{
				whereSql += " AND id = @Id";
			}
			if (!string.IsNullOrEmpty(search.Email))
			{
				whereSql += " AND email LIKE @Email";
			}
			if (!string.IsNullOrEmpty(search.FullName))
			{
				whereSql += " AND full_name LIKE @FullName";
			}
			if (search.RoleId != null)
			{
				whereSql += " AND role_id = @RoleId";
			}
			if (search.StatusAccount != null)
			{
				whereSql += " AND status_account = @StatusAccount";
			}
            if (search.IdLst != null && search.IdLst.Any())
            {
                whereSql += " and id IN @IdLst";
            }
            var userList = await _dbService.GetAll<User>(selectSql + whereSql, search);
			return userList;
		}

        public async Task<List<User>> SearchAdmin(UserSearchDto searchbyadmin)
        {
            var selectSql = "SELECT * FROM account ";
            var whereSql = " WHERE 1=1 ";

            if (searchbyadmin.Id != null)
            {
                whereSql += " AND id = @Id";
            }
            if (!string.IsNullOrEmpty(searchbyadmin.Email))
            {
                whereSql += $" AND email LIKE  N'%{searchbyadmin.Email}%'";
            }
            if (!string.IsNullOrEmpty(searchbyadmin.FullName))
            {
               
                whereSql += $" AND full_name LIKE N'%{searchbyadmin.FullName}%'";
            }
            if (searchbyadmin.RoleId != null)
            {
                whereSql += " AND role_id = @RoleId";
            }
            if (searchbyadmin.StatusAccount != null)
            {
                whereSql += " AND status_account = @StatusAccount";
            }
            if (searchbyadmin.IdLst != null && searchbyadmin.IdLst.Any())
            {
                whereSql += " and id IN @IdLst";
            }
            var userList = await _dbService.GetAll<User>(selectSql + whereSql, searchbyadmin);
            return userList;
        }


        public async Task<bool> Update(User user)
		{
			var updateSql = " UPDATE account SET  ";

			if (user.Email != null)
			{
				updateSql += " email=@Email, ";
			}
			if (user.Password != null)
			{
				updateSql += " password=@Password, ";
			}
			if (user.FullName != null)
			{
				updateSql += " full_name=@FullName, ";
			}
			if (user.UserAddress != null)
			{
				updateSql += " user_address=@UserAddress, ";
			}
			if (user.PhoneNumber != null)
			{
				updateSql += " phone_number=@PhoneNumber, ";
			}
			if (user.StatusAccount != null)
			{
				updateSql += " status_account=@StatusAccount, ";
			}
			if (user.RoleId != null)
			{
				updateSql += " role_id=@RoleId, ";
			}
			if (user.InvalidPasswordCount != -1)
			{
				updateSql += " invalid_password_count=@InvalidPasswordCount, ";
			}
			if (user.DeleteFlag != null)
			{
				updateSql += " delete_flag=@DeleteFlag ";
			}
			else
			{
				if (updateSql.EndsWith(", ")) updateSql = updateSql.Remove(updateSql.Length - 2);
			}

			var whereSql = " WHERE id=@Id ";
			var updateUser = await _dbService.EditData(updateSql + whereSql, user);
			return true;
		}
	}
}
