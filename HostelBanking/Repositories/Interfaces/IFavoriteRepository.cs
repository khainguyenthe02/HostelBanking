using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.Favorite;
using HostelBanking.Entities.Models.Comment;
using HostelBanking.Entities.Models.Favorite;

namespace HostelBanking.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<bool> Create(Favorite favorite);
        Task<List<Favorite>> GetAll();
        Task<bool> Update(Favorite favorite);
        Task<bool> Delete(int id);
        Task<Favorite> GetById(int id);
        Task<List<Favorite>> Search(FavoriteSearchDto search);
    }
}
