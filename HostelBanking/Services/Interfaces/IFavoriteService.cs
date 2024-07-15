using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.Favorite;

namespace HostelBanking.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<bool> Create(FavoriteCreateDto favorite);
        Task<List<FavoriteDto>> GetAll();
        Task<bool> Delete(int id);
        Task<FavoriteDto> GetById(int id);
        Task<bool> Update(FavoriteUpdateDto archives);
        Task<List<FavoriteDto>> Search(FavoriteSearchDto search);
    }
}
