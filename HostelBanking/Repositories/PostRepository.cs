using HostelBanking.SqlServerDbHelper.Interfaces;
using HostelBanking.SqlServerDbHelper;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Entities.Models.Post;
using HostelBanking.Entities.DataTransferObjects.Post;

namespace HostelBanking.Repositories
{
	public class PostRepository : IPostRepository
	{
		private readonly IDbService _dbService;

		public PostRepository(IConfiguration configuration)
		{
			_dbService = new DbService(configuration);
		}

		public async Task<int> Create(Post post)
		{
			var result = await _dbService.EditData(
			"INSERT INTO post (hostel_type_id, account_id, title, price, acreage, dictrict, ward, description_post," +
			" images, create_date, phone_number, owner_house, modified_date, payment_type, delete_flag) " +
			"VALUES (@HostelTypeId, @AccountId, @Title, @Price, @Acreage, @District, @Ward, @DescriptionPost, " +
			"@Images, @CreateDate, @PhoneNumber, @OwnerHouse, @ModifiedDate, @PaymentType, @DeleteFlag)",
			post);
			if (result > 0)
			{
				return post.Id;
			}
			else
			{
				return -1;
			}
		}

		public async Task<bool> Delete(int id)
		{
			var result = await _dbService.EditData("DELETE FROM post WHERE id = @Id", new { Id = id });
			return true;
		}

		public async Task<List<Post>> GetAll()
		{
			var postList = await _dbService.GetAll<Post>("SELECT * FROM post", new { });
			return postList;
		}

		public async Task<Post> GetById(int id)
		{
			var post = await _dbService.GetAsync<Post>("SELECT * FROM post WHERE id = @Id", new { Id = id });
			return post;
		}

		public async Task<Post> GetLatestPost()
		{
			var sql = "SELECT TOP 1 * FROM post ORDER BY id DESC"; // Assuming 'id' is the auto-increment column
			var post = await _dbService.GetAsync<Post>(sql, new { });
			return post;
		}

		public async Task<List<Post>> Search(PostSearchDto search)
		{
			var selectSql = "SELECT * FROM post ";
			var whereSql = " WHERE delete_flag = 0";
			if (search.Id != null)
			{
				whereSql += " AND id = @Id";
			}
			//if (search.HostelTypeId != null)
			//{
			//	whereSql += " AND hostel_type_id = @HostelTypeId";
			//}
			//if (search.AccountId != null)
			//{
			//	whereSql += " AND account_id = @AccountId";
			//}
			if (!string.IsNullOrEmpty(search.Title))
			{
				whereSql += " AND title LIKE @Title";
			}
			if (search.Price != null)
			{
				whereSql += " AND price = @Price";
			}
			if (search.Acreage != null)
			{
				whereSql += " AND acreage = @Acreage";
			}
			if (!string.IsNullOrEmpty(search.District))
			{
				whereSql += " AND dictrict LIKE @District";
			}
			if (!string.IsNullOrEmpty(search.Ward))
			{
				whereSql += " AND ward LIKE @Ward";
			}
			if (!string.IsNullOrEmpty(search.DescriptionPost))
			{
				whereSql += " AND description_post LIKE @DescriptionPost";
			}
			if (!string.IsNullOrEmpty(search.Images))
			{
				whereSql += " AND images LIKE @Images";
			}
			if (search.CreateDate != null)
			{
				whereSql += " AND create_date = @CreateDate";
			}
			if (!string.IsNullOrEmpty(search.PhoneNumber))
			{
				whereSql += " AND phone_number = @PhoneNumber";
			}
			if (!string.IsNullOrEmpty(search.OwnerHouse))
			{
				whereSql += " AND owner_house LIKE @OwnerHouse";
			}
			if (search.ModifiedDate != null)
			{
				whereSql += " AND modified_date = @ModifiedDate";
			}
			if (search.PaymentType != null)
			{
				whereSql += " AND payment_type = @PaymentType";
			}
			var postList = await _dbService.GetAll<Post>(selectSql + whereSql, search);
			return postList;
		}

		public async Task<bool> Update(Post post)
		{
			var updateSql = " UPDATE post SET  ";
			if (post.HostelTypeId != null)
			{
				updateSql += " hostel_type_id=@HostelTypeId, ";
			}
			if (post.AccountId != null)
			{
				updateSql += " account_id=@AccountId, ";
			}
			if (post.Title != null)
			{
				updateSql += " title=@Title, ";
			}
			if (post.Price != null)
			{
				updateSql += " price=@Price, ";
			}
			if (post.Acreage != null)
			{
				updateSql += " acreage=@Acreage, ";
			}
			if (post.District != null)
			{
				updateSql += " dictrict=@District, ";
			}
			if (post.Ward != null)
			{
				updateSql += " ward=@Ward, ";
			}
			if (post.DescriptionPost != null)
			{
				updateSql += " description_post=@DescriptionPost, ";
			}
			if (post.Images != null)
			{
				updateSql += " images=@Images, ";
			}
			if (post.PhoneNumber != null)
			{
				updateSql += " phone_number=@PhoneNumber, ";
			}
			if (post.OwnerHouse != null)
			{
				updateSql += " owner_house=@OwnerHouse, ";
			}
			if (post.ModifiedDate != null)
			{
				updateSql += " modified_date=@ModifiedDate, ";
			}
			if (post.PaymentType != null)
			{
				updateSql += " payment_type=@PaymentType, ";
			}
			if (post.DeleteFlag != null)
			{
				updateSql += " delete_flag=@DeleteFlag ";
			}
			else
			{
				if (updateSql.EndsWith(", ")) updateSql = updateSql.Remove(updateSql.Length - 2);
			}
			var whereSql = " WHERE id=@Id ";
			var updatePost = await _dbService.EditData(updateSql + whereSql, post);
			return true;
		}
	}
}
