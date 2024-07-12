using HostelBanking.Repositories.Interfaces;
using HostelBanking.SqlServerDbHelper.Interfaces;
using HostelBanking.SqlServerDbHelper;
using HostelBanking.Entities.Models.PostImages;
using HostelBanking.Entities.DataTransferObjects.PostImage;
using HostelBanking.Entities.Models.Post;
using Microsoft.Extensions.Hosting;

namespace HostelBanking.Repositories
{
	public class PostImageRepository : IPostImageRepository
	{
		private readonly IDbService _dbService;

		public PostImageRepository(IConfiguration configuration)
		{
			_dbService = new DbService(configuration);
		}
		public async Task<bool> Create(PostImage postImage)
		{
			var result = await _dbService.EditData(
			"INSERT INTO post_image (post_id, image_url, delete_flag) " +
			"VALUES (@PostId, @ImageUrl, @DeleteFlag)",
			postImage);
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
			var result = await _dbService.EditData("DELETE FROM post_image WHERE id = @Id", new { Id = id });
			return true;
		}
		public async Task<List<PostImage>> GetAll()
		{
			var postImageList = await _dbService.GetAll<PostImage>("SELECT * FROM post_image", new { });
			return postImageList;
		}
		public async Task<PostImage> GetById(int id)
		{
			var postImage = await _dbService.GetAsync<PostImage>("SELECT * FROM post_image WHERE id = @Id", new { Id = id });
			return postImage;
		}
		public async Task<List<PostImage>> Search(PostImageSearchDto search)
		{
			var selectSql = "SELECT * FROM post_image ";
			var whereSql = " WHERE delete_flag = 0";
			if (search.Id != null)
			{
				whereSql += " AND id = @Id";
			}
			if (search.PostId != null)
			{
				whereSql += " AND post_id = @PostId";
			}
			if(search.ImageUrl != null)
			{
				whereSql += " AND image_url = @ImageUrl";
			}
			var postImageList = await _dbService.GetAll<PostImage>(selectSql + whereSql, search);
			return postImageList;
		}
		public async Task<bool> Update(PostImage postImage)
		{
			var updateSql = " UPDATE post SET  ";
			if (postImage.ImageUrl != null)
			{
				updateSql += " image_url = @ImageUrl, ";
			}
			if (postImage.DeleteFlag != null)
			{
				updateSql += " delete_flag=@DeleteFlag ";
			}
			else
			{
				if (updateSql.EndsWith(", ")) updateSql = updateSql.Remove(updateSql.Length - 2);
			}
			var whereSql = " WHERE id=@Id ";
			var updateImagePost = await _dbService.EditData(updateSql + whereSql, postImage);
			return true;
		}
	}
}
