using HostelBanking.Repositories.Interfaces;
using HostelBanking.SqlServerDbHelper.Interfaces;
using HostelBanking.SqlServerDbHelper;
using HostelBanking.Entities.Models.Comment;
using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.Models.HostelType;

namespace HostelBanking.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbService _dbService;
        public CommentRepository(IConfiguration configuration)
        {
            _dbService = new DbService(configuration);
        }

        public async Task<bool> Create(Comment comment)
        {
            var result =
            await _dbService.EditData(
              "INSERT INTO comment (account_id, post_id, content, create_date, delete_flag) " +
                "VALUES (@AccountId, @PostId, @Content, @CreateDate,  @DeleteFlag)",
              comment);
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
            var deleteComment = await _dbService.EditData("DELETE FROM comment WHERE id = @Id", new { id });
            return true;
        }

        public async Task<List<Comment>> GetAll()
        {
            var hostelList = await _dbService.GetAll<Comment>("SELECT * FROM comment", new { });
            return hostelList;
        }

        public async Task<Comment> GetById(int id)
        {
            var hostelType = await _dbService.GetAsync<Comment>("SELECT * FROM comment WHERE id = @Id", new { id });
            return hostelType;
        }

        public async Task<List<Comment>> Search(CommentSearchDto search)
        {
            var selectSql = "SELECT * FROM comment";

            var whereSql = " WHERE delete_flag = 0";

            if (search.Id != null)
            {
                whereSql += " AND id = @Id";
            }
            if (search.PostId != null)
            {
                whereSql += " AND post_id = @PostId";
            }
            if (search.AccountId != null )
            {
                whereSql += " AND account_id = @AccountId";
            }
            if (search.Content != null)
            {
                whereSql += " AND content = @Content";
            }
            if (search.CreateDate != null)
            {
                whereSql += " AND create_date > @CreateDate";
            }

            var hostelTypeLst = await _dbService.GetAll<Comment>(selectSql + whereSql, search);

            return hostelTypeLst;
        }

        public async Task<bool> Update(Comment comment)
        {
            var updateSql = " UPDATE comment SET  ";
            if (comment.PostId != null)
            {
                updateSql += " post_id=@PostId, ";
            }
            if (comment.AccountId != null)
            {
                updateSql += " account_id=@AccountId, ";
            }
            if (comment.Content != null)
            {
                updateSql += " content=@Content, ";
            }
            if (comment.CreateDate != null)
            {
                updateSql += " create_date=@CreateDate, ";
            }
            if (comment.DeleteFlag != null)
            {
                updateSql += " delete_flag=@DeleteFlag ";
            }
            else
            {
                if (updateSql.EndsWith(", ")) updateSql = updateSql.Remove(updateSql.Length - 2);
            }
            var whereSql = " WHERE id=@Id ";

            var updateHostelType =
            await _dbService.EditData(updateSql + whereSql, comment);
            return true;
        }
    }
}
