using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.DataTransferObjects.PostImage;
using HostelBanking.Entities.Models.Post;
using HostelBanking.Entities.Models.PostImages;

namespace HostelBanking.Repositories.Interfaces
{
	public interface IPostImageRepository
	{
		Task<bool> Create(PostImage postImage);
		Task<List<PostImage>> GetAll();
		Task<bool> Update(PostImage postImage);
		Task<bool> Delete(string id);
		Task<PostImage> GetById(int id);
		Task<List<PostImage>> Search(PostImageSearchDto search);
	}
}
