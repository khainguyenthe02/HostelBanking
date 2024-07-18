using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.Favorite;
using HostelBanking.Entities.DataTransferObjects.PayHistory;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.DataTransferObjects.PostImage;
using HostelBanking.Entities.Enum;
using HostelBanking.Entities.Models.Favorite;
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
            if (result)
            {
                var postId = postImageInfo.PostId;
                var post = await _repositoryManager.PostRepository.GetById(postId);
                if (post != null)
                {
                    post.PaymentType = (int)PaymentStatus.PAID;
                    var resultDto = await _repositoryManager.PostRepository.Update(post);
                    return resultDto;
                }
            }
            return false;
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
			var resultDto = result.Adapt<List<PayHistoryDto>>();
			return await FilterData(resultDto);
		}

        public async Task<PayHistoryDto> GetById(int id)
        {
            var result = await _repositoryManager.PayHistoryRepository.GetById(id);
            var resultDto =  result.Adapt<PayHistoryDto>();
			return (await FilterData(new() { resultDto })).FirstOrDefault();
		}

        public async Task<List<PayHistoryDto>> Search(PayHistorySearchDto search)
        {
            var result = await _repositoryManager.PayHistoryRepository.Search(search);
            var resultDto = result.Adapt<List<PayHistoryDto>>();
            return await FilterData(resultDto);
        }

        public async Task<bool> Update(PayHistoryUpdateDto payHistory)
        {
            var hostelTypeInfo = payHistory.Adapt<PayHistory>();
            var result = await _repositoryManager.PayHistoryRepository.Update(hostelTypeInfo);
            return result;
        }
        public async Task<List<PayHistoryDto>> FilterData(List<PayHistoryDto> lst)
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
