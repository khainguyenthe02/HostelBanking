using HostelBanking.Entities.DataTransferObjects;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.SqlServerDbHelper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Data;

namespace HostelBanking.Repositories
{
	public class HostelRepository : IHostelTypeRepository
	{
		private readonly SqlServerBase _dbService;

		public HostelRepository(IConfiguration configuration)
		{
			_dbService = new SqlServerBase(configuration);
		}
		public async Task<bool> Create(HostelType hostelType)
		{
			var result =
			await _dbService.EditData(
			  "INSERT INTO tblHostelType (sType, delete_flag) " +
			  "VALUES (@TypeName, @DeleteFlag)",
			  hostelType);
			if (result > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> Delete(string id)
		{
			var deleteHostelType = await _dbService.EditData("DELETE FROM tblHostelType WHERE PK_iHostelTypeId = @Id", new { id });
			return true;
		}

		public async Task<List<HostelType>> GetAll()
		{
			var hostelList = await _dbService.GetAll<HostelType>("SELECT * FROM tblHostelType", new { });
			return hostelList;
		}

		public async Task<HostelType> GetById(int id)
		{
			var hostelType = await _dbService.GetAsync<HostelType>("SELECT * FROM tblHostelType PK_iHostelTypeId=@Id", new { id });
			return hostelType;
		}	
		public Task<List<HostelType>> Search(HostelTypeSearchDto search)
		{
			throw new NotImplementedException();
		}

		public async Task<HostelType> Update(HostelType hostelType)
		{
			var updateSql = " Update tblHostelType SET ";
			if (hostelType.TypeName != null)
			{
				updateSql += " sType=@TypeName, ";
			}
			if (hostelType.DeleteFlag != null)
			{
				updateSql += " delete_flag=@DeletFlag ";
			}
			else
			{
				if (updateSql.EndsWith(", ")) updateSql = updateSql.Remove(updateSql.Length - 2);
			}
			var whereSql = " WHERE PK_iHostelTypeId=@Id ";

			var updateHostelType =
			await _dbService.EditData(updateSql + whereSql, hostelType);
			return hostelType;
		}
	}
}
