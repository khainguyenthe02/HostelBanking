using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.Favorite;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.Models.Comment;
using HostelBanking.Entities.Models.Favorite;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;

namespace HostelBanking.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IRepositoryManager _repositoryManager;
        public FavoriteService(IRepositoryManager repositoryManager)
        {
            this._repositoryManager = repositoryManager;
        }

        public async Task<bool> Create(FavoriteCreateDto favorite)
        {
            var favoriteInfo = favorite.Adapt<Favorite>();
            favoriteInfo.CreateDate = DateTime.Now;
            var result = await _repositoryManager.FavoriteRepository.Create(favoriteInfo);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            FavoriteSearchDto search = new()
            {
                Id = id,
            };
            var hostelTypeInfo = await _repositoryManager.FavoriteRepository.Search(search);
            if (hostelTypeInfo != null)
            {
                var hostelTypeUpdate = new Favorite();
                hostelTypeUpdate.Id = id;
                hostelTypeUpdate.DeleteFlag = true;
                var result = await _repositoryManager.FavoriteRepository.Update(hostelTypeUpdate);
                return true;
            }
            return false;
        }

        public async Task<List<FavoriteDto>> GetAll()
        {
            var result = await _repositoryManager.FavoriteRepository.GetAll();
            return result.Adapt<List<FavoriteDto>>();
        }

        public async Task<FavoriteDto> GetById(int id)
        {
            var result = await _repositoryManager.FavoriteRepository.GetById(id);
            return result.Adapt<FavoriteDto>();
        }

        public async Task<List<FavoriteDto>> Search(FavoriteSearchDto search)
        {
            var result = await _repositoryManager.FavoriteRepository.Search(search);    
			var resultDto = result.Adapt<List<FavoriteDto>>();
			return await FilterData(resultDto);
		}

        public async Task<bool> Update(FavoriteUpdateDto archives)
        {
            var hostelTypeInfo = archives.Adapt<Favorite>();
            var result = await _repositoryManager.FavoriteRepository.Update(hostelTypeInfo);
            return result;
        }
		public async Task<List<FavoriteDto>> FilterData(List<FavoriteDto> lst)
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
