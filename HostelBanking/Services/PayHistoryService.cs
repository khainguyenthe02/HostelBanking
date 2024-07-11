using HostelBanking.Entities.DataTransferObjects.PayHistory;
using HostelBanking.Entities.DataTransferObjects.PostImage;
using HostelBanking.Entities.Models.PayHistory;
using HostelBanking.Entities.Models.PostImages;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;

namespace HostelBanking.Services
{
    public class PayHistoryService : IPayHistoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        public PayHistoryService(IRepositoryManager repositoryManager)
        {
            this._repositoryManager = repositoryManager;
        }

        public async Task<bool> Create(PayHistoryCreateDto payHistory)
        {
            var postImageInfo = payHistory.Adapt<PayHistory>();
            var result = await _repositoryManager.PayHistoryRepository.Create(postImageInfo);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            PayHistorySearchDto search = new()
            {
                Id = id,
            };
            var payHistoryInfo = await _repositoryManager.PayHistoryRepository.Search(search);
            if (payHistoryInfo != null)
            {
                var payHistoryUpdate = new PayHistory();
                payHistoryUpdate.Id = id;
                payHistoryUpdate.DeleteFlag = true;
                var result = await _repositoryManager.PayHistoryRepository.Update(payHistoryUpdate);
                return true;
            }
            return false;
        }

        public async Task<List<PayHistoryDto>> GetAll()
        {
            var result = await _repositoryManager.PayHistoryRepository.GetAll();
            return result.Adapt<List<PayHistoryDto>>();
        }

        public async Task<PayHistoryDto> GetById(int id)
        {
            var result = await _repositoryManager.PayHistoryRepository.GetById(id);
            return result.Adapt<PayHistoryDto>();
        }

        public Task<List<PayHistoryDto>> Search(PayHistorySearchDto search)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(PayHistoryUpdateDto payHistory)
        {
            throw new NotImplementedException();
        }
    }
}
