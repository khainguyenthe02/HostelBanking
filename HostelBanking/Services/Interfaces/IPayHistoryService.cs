using HostelBanking.Entities.DataTransferObjects.PayHistory;

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
        Task<List<PayHistoryDto>> GetlastPayOfPost(int id);
    }
}
