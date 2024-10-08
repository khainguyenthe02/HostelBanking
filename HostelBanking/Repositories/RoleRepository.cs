﻿using HostelBanking.Entities.DataTransferObjects.Roles;
using HostelBanking.Entities.Models.Roles;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.SqlServerDbHelper.Interfaces;
using HostelBanking.SqlServerDbHelper;
using HostelBanking.Entities.Models.HostelType;

namespace HostelBanking.Repositories
{
	public class RoleRepository : IRoleRepository
	{
		private readonly IDbService _dbService;

		public RoleRepository(IConfiguration configuration)
		{
			_dbService = new DbService(configuration);
		}
		public async Task<bool> Create(Roles roles)
		{
			var result =
			await _dbService.EditData(
			  "INSERT INTO roles (role_name, information, delete_flag) " +
				"VALUES (@RoleName, @Information, @DeleteFlag)",
			  roles);
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
			var deleteRole = await _dbService.EditData("DELETE FROM roles WHERE id = @Id", new { id });
			return true;
		}

		public async Task<List<Roles>> GetAll()
		{
			var roleList = await _dbService.GetAll<Roles>("SELECT * FROM roles", new { });
			return roleList;
		}

		public async Task<Roles> GetById(int id)
		{
			var role = await _dbService.GetAsync<Roles>("SELECT * FROM roles WHERE id = @Id", new { id });
			return role;
		}

		public async Task<List<Roles>> Search(RoleSearchDto search)
		{
			var selectSql = "SELECT * FROM roles";

			var whereSql = " WHERE delete_flag = 0";

			if (search.Id != null)
			{
				whereSql += " AND id = @Id";
			}
			if (search.RoleName != null)
			{
				whereSql += " AND role_name = @RoleName";
			}
			if (search.IdLst != null && search.IdLst.Any())
			{
				whereSql += " and id IN @IdLst";
			}

			var roleLst = await _dbService.GetAll<Roles>(selectSql + whereSql, search);

			return roleLst;
		}

		public async Task<bool> Update(Roles roles)
		{
			var updateSql = " UPDATE roles SET  ";
			if (roles.RoleName != null)
			{
				updateSql += " role_name=@RoleName, ";
			}
			if (roles.Information != null)
			{
				updateSql += " information=@Information, ";
			}
			if (roles.DeleteFlag != null)
			{
				updateSql += " delete_flag=@DeleteFlag ";
			}
			else
			{
				if (updateSql.EndsWith(", ")) updateSql = updateSql.Remove(updateSql.Length - 2);
			}
			var whereSql = " WHERE id=@Id ";

			var updateRole =
			await _dbService.EditData(updateSql + whereSql, roles);
			return true;
		}
	}
}
