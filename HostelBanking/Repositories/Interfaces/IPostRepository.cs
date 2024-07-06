using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Entities.Models.Post;

namespace HostelBanking.Repositories.Interfaces
{
	public interface IPostRepository
	{
		Task<bool> Create(Post post);
		Task<List<Post>> GetAll();
		Task<bool> Update(Post post);
		Task<bool> Delete(string id);
		Task<Post> GetById(int id);
		Task<List<Post>> Search(PostSearchDto search);
	}
}
