using HostelBanking.Entities.DataTransferObjects.PayHistory;
using HostelBanking.Entities.DataTransferObjects.PostImage;

namespace HostelBanking.Services.Interfaces
{
    public interface IPayHistoryService
    {
        Task<bool> Create(PayHistoryCreateDto payHistory);
        Task<List<PayHistoryDto>> GetAll();
        Task<bool> Delete(int id);
        Task<PayHistoryDto> GetById(int id);
        Task<bool> Update(PayHistoryUpdateDto payHistory);
        Task<List<PayHistoryDto>> Search(PayHistorySearchDto search);
    }
}
