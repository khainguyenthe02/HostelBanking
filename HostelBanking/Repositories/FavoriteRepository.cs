using HostelBanking.Repositories.Interfaces;
using HostelBanking.SqlServerDbHelper.Interfaces;
using HostelBanking.SqlServerDbHelper;
using HostelBanking.Entities.Models.Favorite;
using HostelBanking.Entities.DataTransferObjects.Favorite;
using HostelBanking.Entities.Models.Comment;

namespace HostelBanking.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly IDbService _dbService;
        public FavoriteRepository(IConfiguration configuration)
        {
            _dbService = new DbService(configuration);
        }

        public async Task<bool> Create(Favorite favorite)
        {
            var result =
            await _dbService.EditData(
              "INSERT INTO favorite (account_id, post_id, create_date, delete_flag) " +
                "VALUES (@AccountId, @PostId, @CreateDate,  @DeleteFlag)",
              favorite);
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
            var deleteFavorite = await _dbService.EditData("DELETE FROM favorite WHERE id = @Id", new { id });
            return true;
        }

        public async Task<List<Favorite>> GetAll()
        {
            var hostelList = await _dbService.GetAll<Favorite>("SELECT * FROM favorite", new { });
            return hostelList;
        }

        public async Task<Favorite> GetById(int id)
        {
            var hostelType = await _dbService.GetAsync<Favorite>("SELECT * FROM favorite WHERE id = @Id", new { id });
            return hostelType;
        }

        public async Task<List<Favorite>> Search(FavoriteSearchDto search)
        {
            var selectSql = "SELECT * FROM favorite";

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
            if (search.CreateDate != null)
            {
                whereSql += " AND create_date > @CreateDate";
            }

            var hostelTypeLst = await _dbService.GetAll<Favorite>(selectSql + whereSql, search);

            return hostelTypeLst;
        }

        public async Task<bool> Update(Favorite favorite)
        {
            var updateSql = " UPDATE favorite SET  ";
            if (favorite.PostId != null)
            {
                updateSql += " post_id=@PostId, ";
            }
            if (favorite.AccountId != null)
            {
                updateSql += " account_id=@AccountId, ";
            }
            if (favorite.DeleteFlag != null)
            {
                updateSql += " delete_flag=@DeleteFlag ";
            }
            else
            {
                if (updateSql.EndsWith(", ")) updateSql = updateSql.Remove(updateSql.Length - 2);
            }
            var whereSql = " WHERE id=@Id ";

            var updateHostelType =
            await _dbService.EditData(updateSql + whereSql, favorite);
            return true;
        }
    }
}
