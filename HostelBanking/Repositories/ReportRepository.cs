using HostelBanking.Repositories.Interfaces;
using HostelBanking.SqlServerDbHelper.Interfaces;
using HostelBanking.SqlServerDbHelper;
using HostelBanking.Entities.Models.Report;
using HostelBanking.Entities.DataTransferObjects.Report;
using HostelBanking.Entities.Models.Comment;

namespace HostelBanking.Repositories
{
	public class ReportRepository : IReportRepository
	{
		private readonly IDbService _dbService;
		public ReportRepository(IConfiguration configuration)
		{
			_dbService = new DbService(configuration);
		}

		public async Task<bool> Create(Report report)
		{
			var result =
			await _dbService.EditData(
			  "INSERT INTO report (account_id, post_id, report_status, detail,, create_date, delete_flag) " +
				"VALUES (@AccountId, @PostId, @ReportStatus, @Detail, @CreateDate, @DeleteFlag)",
			  report);
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
			var deleteReport = await _dbService.EditData("DELETE FROM report WHERE id = @Id", new { id });
			return true;
		}

		public async Task<List<Report>> GetAll()
		{
			var hostelList = await _dbService.GetAll<Report>("SELECT * FROM report", new { });
			return hostelList;
		}

		public async Task<Report> GetById(int id)
		{
			var hostelType = await _dbService.GetAsync<Report>("SELECT * FROM report WHERE id = @Id", new { id });
			return hostelType;
		}

		public async Task<List<Report>> Search(ReportSearchDto search)
		{
			var selectSql = "SELECT * FROM report";

			var whereSql = " WHERE delete_flag = 0";

			if (search.Id != null)
			{
				whereSql += " AND id = @Id";
			}
			if (search.PostId != null)
			{
				whereSql += " AND post_id = @PostId";
			}
			if (search.AccountId != null)
			{
				whereSql += " AND account_id = @AccountId";
			}
			if (search.ReportStatus != null)
			{
				whereSql += " AND report_status = @ReportStatus";
			}

			var hostelTypeLst = await _dbService.GetAll<Report>(selectSql + whereSql, search);

			return hostelTypeLst;
		}

		public async Task<bool> Update(Report report)
		{
			var updateSql = " UPDATE report SET  ";
			if (report.PostId != null)
			{
				updateSql += " post_id=@PostId, ";
			}
			if (report.AccountId != null)
			{
				updateSql += " account_id=@AccountId, ";
			}
			if (report.Detail != null)
			{
				updateSql += " detail=@Detail, ";
			}
			if (report.ReportStatus != null)
			{
				updateSql += " report_status=@ReportStatus, ";
			}
			if (report.CreateDate != null)
			{
				updateSql += " create_date=@CreateDate, ";
			}
			if (report.DeleteFlag != null)
			{
				updateSql += " delete_flag=@DeleteFlag ";
			}
			else
			{
				if (updateSql.EndsWith(", ")) updateSql = updateSql.Remove(updateSql.Length - 2);
			}
			var whereSql = " WHERE id=@Id ";

			var updateHostelType =
			await _dbService.EditData(updateSql + whereSql, report);
			return true;
		}
	}
}
