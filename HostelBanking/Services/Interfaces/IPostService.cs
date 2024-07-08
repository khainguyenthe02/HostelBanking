using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.DataTransferObjects.Roles;

namespace HostelBanking.Services.Interfaces
{
	public interface IPostService
	{
		Task<bool> Create(PostCreateDto post);
		Task<List<PostDto>> GetAll();
		Task<bool> Delete(int id);
		Task<PostDto> GetById(int id);
		Task<bool> Update(PostUpdateDto post);
		Task<List<PostDto>> Search(PostSearchDto search);
	}
}
