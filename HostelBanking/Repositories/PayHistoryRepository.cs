using HostelBanking.SqlServerDbHelper.Interfaces;
using HostelBanking.SqlServerDbHelper;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Entities.Models.PayHistory;
using HostelBanking.Entities.DataTransferObjects.PayHistory;
using HostelBanking.Entities.Models.PostImages;

namespace HostelBanking.Repositories
{
    public class PayHistoryRepository : IPayHistoryRepository
    {
        private readonly IDbService _dbService;

        public PayHistoryRepository(IConfiguration configuration)
        {
            _dbService = new DbService(configuration);
        }

        public async Task<bool> Create(PayHistory payHistory)
        {
            var result = await _dbService.EditData(
            "INSERT INTO pay_history (pay_code, pay_date, post_id, account_id, price, type, delete_flag) " +
            "VALUES (@PayCode, @PayDate, @PostId, @AccountId, @Price, @Type, @DeleteFlag)",
            payHistory);
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
            var result = await _dbService.EditData("DELETE FROM pay_history WHERE id = @Id", new { Id = id });
            return true;
        }

        public async Task<List<PayHistory>> GetAll()
        {
            var payHistoryList = await _dbService.GetAll<PayHistory>("SELECT * FROM pay_history", new { });
            return payHistoryList;
        }

        public async Task<PayHistory> GetById(int id)
        {
            var payHistory = await _dbService.GetAsync<PayHistory>("SELECT * FROM pay_history WHERE id = @Id", new { Id = id });
            return payHistory;
        }

        public async Task<List<PayHistory>> Search(PayHistorySearchDto search)
        {
            var selectSql = "SELECT * FROM pay_history ";
            var whereSql = " WHERE delete_flag = 0";
            if (search.Id != null)
            {
                whereSql += " AND id = @Id";
            }
            if (search.PostId != null)
            {
                whereSql += " AND post_id = @PostId";
            }
            if (search.PayCode != null)
            {
                whereSql += " AND pay_code = @PayCode";
            }
            if (search.PayDate != null)
            {
                whereSql += " AND pay_date > @PayDate";
            }
            if(search.AccountId != null)
            {
                whereSql += " AND account_id=@AccountId";
            }
            if (search.Type != null)
            {
                whereSql += " AND type=@Type";
            }
            var postImageList = await _dbService.GetAll<PayHistory>(selectSql + whereSql, search);
            return postImageList;
        }

        public async Task<bool> Update(PayHistory payHistory)
        {
            var updateSql = " UPDATE pay_history SET  ";
            if (payHistory.PostId != null)
            {
                updateSql += " post_id = @PostId, ";
            }
            if (payHistory.PayCode != null)
            {
                updateSql += " pay_code = @PayCode, ";
            }
            if (payHistory.PayDate != null)
            {
                updateSql += " pay_date = @PayDate, ";
            }
            if (payHistory.AccountId != null)
            {
                updateSql += " account_id=@AccountId, ";
            }           
            if (payHistory.Type != null)
            {
                updateSql += " type=@Type, ";
            }
            if (payHistory.DeleteFlag != null)
            {
                updateSql += " delete_flag=@DeleteFlag ";
            }
            else
            {
                if (updateSql.EndsWith(", ")) updateSql = updateSql.Remove(updateSql.Length - 2);
            }
            var whereSql = " WHERE id=@Id ";
            var updateImagePost = await _dbService.EditData(updateSql + whereSql, payHistory);
            return true;
        }
    }
}
