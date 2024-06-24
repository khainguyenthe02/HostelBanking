using HostelBanking.SqlServerDbHelper.Interfaces;
using Serilog;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace HostelBanking.SqlServerDbHelper
{
	public class SqlServerBase : ISqlServerBase
	{
		private string sqlServerConnectionString;
		public SqlServerBase(IConfiguration configuration)
		{
			sqlServerConnectionString = configuration.GetValue<string>("SqlServer:ConnectionString", "localhost:8080");
		}
		public async Task<int> EditData(string command, object parms)
		{
			try
			{
				using (var connection = new SqlConnection(sqlServerConnectionString))
				{
					await connection.OpenAsync();
					int result;

					result = await connection.ExecuteAsync(command, parms);

					return result; ;
				}

			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
				return -1;
			}
		}

		public async Task<List<T>> GetAll<T>(string command, object parms)
		{
			try
			{

				using (var connection = new SqlConnection(sqlServerConnectionString))
				{
					await connection.OpenAsync();
					List<T> result = new List<T>();
					result = (await connection.QueryAsync<T>(command, parms)).ToList();

					return result;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
				return default(List<T>);
			}
		}

		public async Task<T> GetAsync<T>(string command, object parms)
		{
			try
			{
				using (var connection = new SqlConnection(sqlServerConnectionString))
				{
					await connection.OpenAsync();
					T result;
					result = (await connection.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();
					return result;
				}

			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
				return default(T);
			}
		}
		public async Task<int> ExecuteScalar(string command, object parms)
		{
			try
			{
				using (var connection = new SqlConnection(sqlServerConnectionString))
				{
					await connection.OpenAsync();
					var result = (await connection.ExecuteScalarAsync<int>(command, parms));

					return result;
				}

			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
				return -1;
			}
		}
	}
}

