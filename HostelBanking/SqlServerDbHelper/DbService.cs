using HostelBanking.SqlServerDbHelper.Interfaces;
using Serilog;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Configuration;

namespace HostelBanking.SqlServerDbHelper
{
	public class DbService : IDbService
	{
		private readonly IDbConnection _db;

		public DbService(IConfiguration configuration)
		{
			_db = new SqlConnection(configuration.GetConnectionString("HostelBanking"));
		}

		public async Task<T> GetAsync<T>(string command, object parms)
		{
			T result;

			result = (await _db.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();

			return result;

		}

		public async Task<List<T>> GetAll<T>(string command, object parms)
		{

			List<T> result = new List<T>();

			result = (await _db.QueryAsync<T>(command, parms)).ToList();

			return result;
		}

		public async Task<int> EditData(string command, object parms)
		{
			int result;

			result = await _db.ExecuteAsync(command, parms);

			return result;
		}
		public async Task<int> ExecuteScalar(string command, object parms)
		{
				int result;
				result = await _db.ExecuteScalarAsync<int>(command, parms);
				return result;
		}
	}
}

