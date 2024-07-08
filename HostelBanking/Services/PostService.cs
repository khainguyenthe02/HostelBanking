using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;

namespace HostelBanking.Services
{
	public class PostService : IPostService
	{
		private readonly IRepositoryManager _repositoryManager;
		public PostService(IRepositoryManager repositoryManager)
		{
			this._repositoryManager = repositoryManager;
		}

		public Task<bool> Create(PostCreateDto post)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<PostDto>> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task<PostDto> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<PostDto>> Search(PostSearchDto search)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Update(PostUpdateDto post)
		{
			throw new NotImplementedException();
		}
	}
}
