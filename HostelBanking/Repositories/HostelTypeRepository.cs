using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.SqlServerDbHelper;
using HostelBanking.SqlServerDbHelper.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Data;

namespace HostelBanking.Repositories
{
    public class HostelTypeRepository : IHostelTypeRepository
	{
		private readonly IDbService _dbService;

		public HostelTypeRepository(IConfiguration configuration)
		{
			_dbService = new DbService(configuration);
		}
		public async Task<bool> Create(HostelType hostelType)
		{
			var result =
			await _dbService.EditData(
			  "INSERT INTO hostel_type (hostel_type_name, information, delete_flag) " +
				"VALUES (@HostelTypeName, @Information,  @DeleteFlag)",
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

		public async Task<bool> Delete(int id)
		{
			var deleteHostelType = await _dbService.EditData("DELETE FROM hostel_type WHERE id = @Id", new { id });
			return true;
		}

		public async Task<List<HostelType>> GetAll()
		{
			var hostelList = await _dbService.GetAll<HostelType>("SELECT * FROM hostel_type", new { });
			return hostelList;
		}

		public async Task<HostelType> GetById(int id)
		{
			var hostelType = await _dbService.GetAsync<HostelType>("SELECT * FROM hostel_type WHERE id = @Id", new { id });
			return hostelType;
		}	
		public async Task<List<HostelType>> Search(HostelTypeSearchDto search)
		{
			var selectSql = "SELECT * FROM hostel_type";

			var whereSql = " WHERE delete_flag = 0"; 

			if (search.Id != null)
			{
				whereSql += " AND id = @Id";
			}
			if (search.HostelTypeName != null)
			{
				whereSql += " AND hostel_type_name = @HostelTypeName";
			}
            if (search.IdLst != null && search.IdLst.Any())
            {
                whereSql += " and id IN @IdLst";
            }

            var hostelTypeLst = await _dbService.GetAll<HostelType>(selectSql + whereSql, search);

				return hostelTypeLst;
		}

		public async Task<bool> Update(HostelType hostelType)
		{
			var updateSql = " UPDATE hostel_type SET  ";
			if (hostelType.HostelTypeName != null)
			{
				updateSql += " hostel_type_name=@HostelTypeName, ";
			}
			if(hostelType.Information != null)
			{
				updateSql += " information=@Information, ";
			}
			if (hostelType.DeleteFlag != null)
			{
				updateSql += " delete_flag=@DeletFlag ";
			}
			else
			{
				if (updateSql.EndsWith(", ")) updateSql = updateSql.Remove(updateSql.Length - 2);
			}
			var whereSql = " WHERE id=@Id ";

			var updateHostelType =
			await _dbService.EditData(updateSql + whereSql, hostelType);
			return true;
		}
	}
}
