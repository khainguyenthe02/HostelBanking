using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.Models.Post;
using HostelBanking.Entities.Models.PostImages;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace HostelBanking.Services
{
	public class PostService : IPostService
	{
		private readonly IRepositoryManager _repositoryManager;
		public PostService(IRepositoryManager repositoryManager)
		{
			this._repositoryManager = repositoryManager;
		}

		public async Task<Post> Create(PostCreateDto post)
		{
            var postInfo = post.Adapt<Post>();
            var result = await _repositoryManager.PostRepository.Create(postInfo);
            return postInfo;
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

		public async Task<Post> GetLatestPost()
		{
			return await _repositoryManager.PostRepository.GetLatestPost();
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
