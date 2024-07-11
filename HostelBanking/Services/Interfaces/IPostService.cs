using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.DataTransferObjects.Roles;
using HostelBanking.Entities.Models.Post;

namespace HostelBanking.Services.Interfaces
{
	public interface IPostService
	{
		Task<Post> Create(PostCreateDto post);
		Task<List<PostDto>> GetAll();
		Task<Post> GetLatestPost();
		Task<bool> Delete(int id);
		Task<PostDto> GetById(int id);
		Task<bool> Update(PostUpdateDto post);
		Task<List<PostDto>> Search(PostSearchDto search);
		Task<List<PostDto>> GetNewest();
		Task<List<PostDto>> GetMostView();
    }
}
