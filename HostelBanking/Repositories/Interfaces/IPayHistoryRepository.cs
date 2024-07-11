using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.PayHistory;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Entities.Models.PayHistory;

namespace HostelBanking.Repositories.Interfaces
{
    public interface IPayHistoryRepository
    {
        Task<bool> Create(PayHistory payHistory);
        Task<List<PayHistory>> GetAll();
        Task<bool> Update(PayHistory payHistory);
        Task<bool> Delete(int id);
        Task<PayHistory> GetById(int id);
        Task<List<PayHistory>> Search(PayHistorySearchDto search);
    }
}
