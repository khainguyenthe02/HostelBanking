using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.Favorite;
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
            return result.Adapt<List<FavoriteDto>>();
        }

        public async Task<bool> Update(FavoriteUpdateDto archives)
        {
            var hostelTypeInfo = archives.Adapt<Favorite>();
            var result = await _repositoryManager.FavoriteRepository.Update(hostelTypeInfo);
            return result;
        }
    }
}
