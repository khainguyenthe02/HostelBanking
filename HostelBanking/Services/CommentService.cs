using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.Favorite;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.Models.Comment;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;

namespace HostelBanking.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepositoryManager _repositoryManager;
        public CommentService(IRepositoryManager repositoryManager)
        {
            this._repositoryManager = repositoryManager;
        }

        public async Task<bool> Create(CommentCreateDto comment)
        {
            var hostelTypeInfo = comment.Adapt<Comment>();
            var result = await _repositoryManager.CommentRepository.Create(hostelTypeInfo);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            CommentSearchDto search = new()
            {
                Id = id,
            };
            var hostelTypeInfo = (await _repositoryManager.CommentRepository.Search(search)).FirstOrDefault();
            if (hostelTypeInfo != null)
            {
                var hostelTypeUpdate = new Comment();
                hostelTypeUpdate.Id = id;
				hostelTypeUpdate.AccountId = hostelTypeInfo.AccountId;
				hostelTypeUpdate.PostId = hostelTypeInfo.PostId;
                hostelTypeUpdate.DeleteFlag = true;
                var result = await _repositoryManager.CommentRepository.Update(hostelTypeUpdate);
                return true;
            }
            return false;
        }

        public async Task<List<CommentDto>> GetAll()
        {
            var result = await _repositoryManager.CommentRepository.GetAll();
			var resultDto = result.Adapt<List<CommentDto>>();
			return await FilterData(resultDto);
		}

        public async Task<CommentDto> GetById(int id)
        {
            var result = await _repositoryManager.CommentRepository.GetById(id);
            var resultDto = result.Adapt<CommentDto>();
             return (await FilterData(new() { resultDto })).FirstOrDefault();
        }

        public async Task<List<CommentDto>> Search(CommentSearchDto search)
        {
            var result = await _repositoryManager.CommentRepository.Search(search);
			var resultDto = result.Adapt<List<CommentDto>>();
			return await FilterData(resultDto);
		}

        public async Task<bool> Update(CommentUpdateDto comment)
        {
            var hostelTypeInfo = comment.Adapt<Comment>();
            var result = await _repositoryManager.CommentRepository.Update(hostelTypeInfo);
            return result;
        }
		public async Task<List<CommentDto>> FilterData(List<CommentDto> lst)
		{
			if (lst?.Count > 0)
			{
				var userIdLst = lst.Where(x => x.AccountId.HasValue).Select(x => x.AccountId.GetValueOrDefault()).ToList();
				if (userIdLst.Count > 0)
				{
					var searchUser = new UserSearchDto()
					{
						IdLst = userIdLst
					};
					var users = (await _repositoryManager.UserRepository.Search(searchUser))?.ToDictionary(x => x.Id, x => x.FullName);
					if (users?.Count > 0)
					{
						foreach (var item in lst)
						{
							if (item.AccountId.HasValue && users.ContainsKey(item.AccountId.Value))
							{
								item.AccountName = users[item.AccountId.Value];
							}
						}
					}
				}
				var postIdLst = lst.Where(x => x.PostId.HasValue).Select(x => x.PostId.GetValueOrDefault()).ToList();
				if (postIdLst.Count > 0)
				{
					var searchPost = new PostSearchDto()
					{
						IdLst = postIdLst
					};
					var posts = (await _repositoryManager.PostRepository.Search(searchPost))?.ToDictionary(x => x.Id, x => x.Title);
					if (posts?.Count > 0)
					{
						foreach (var item in lst)
						{
							if (item.PostId.HasValue && posts.ContainsKey(item.PostId.Value))
							{
								item.PostTitle = posts[item.PostId.Value];
							}
						}
					}
				}
			}
			return lst;
		}
	}
}
