using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.DataTransferObjects.PostImage;
using HostelBanking.Entities.DataTransferObjects.Roles;
using HostelBanking.Entities.Enum;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Entities.Models.Post;
using HostelBanking.Entities.Models.PostImages;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;
using Mapster.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace HostelBanking.Services
{
    public class PostService : IPostService
    {
        private readonly IRepositoryManager _repositoryManager;
        public PostService(IRepositoryManager repositoryManager)
        {
            this._repositoryManager = repositoryManager;
        }

        public async Task<Post> Create(PostCreateDto post)
        {
            var postInfo = post.Adapt<Post>();
            postInfo.CreateDate = DateTime.Now;
            postInfo.ModifiedDate = DateTime.Now;
            postInfo.CountViews = 0;
            postInfo.Images = string.Join(",", post.Images);
            postInfo.PaymentType = (int)PaymentStatus.PENDING;

            var result = await _repositoryManager.PostRepository.Create(postInfo);
            return postInfo;

        }

        public async Task<bool> Delete(int id)
        {
            PostSearchDto search = new()
            {
                Id = id,
            };
            var postInfo = await _repositoryManager.PostRepository.Search(search);
            if (postInfo != null)
            {
                var postUpdate = postInfo[0];
                postUpdate.Id = id;
                postUpdate.DeleteFlag = true;
                var result = await _repositoryManager.PostRepository.Update(postUpdate);
                return result;
            }
            return false;
        }

        public async Task<List<PostDto>> GetAll()
        {
            var result = await _repositoryManager.PostRepository.GetAll();
            var resultDto = result.Adapt<List<PostDto>>();

            return await FilterData(resultDto);
        }
        public async Task<List<PostDto>> GetNewest()
        {
            var result = await _repositoryManager.PostRepository.GetNewest();
            result.ForEach((post) =>
            {
                post.Adapt<PostDto>().Images=post.Images.Split(',').ToList();
            });
           
            var resultDto = result.Adapt<List<PostDto>>();
			return await FilterData(resultDto);
		}

        public async Task<List<PostDto>> GetMostView()
        {
            var result = await _repositoryManager.PostRepository.GetNewest();
            result.ForEach((post) =>
            {
                post.Adapt<PostDto>().Images = post.Images.Split(',').ToList();
            });

			var resultDto = result.Adapt<List<PostDto>>();
			return await FilterData(resultDto);
		}

        public async Task<PostDto> GetById(int id)
        {
            var post = await _repositoryManager.PostRepository.GetById(id);
            if (post == null) return new PostDto();  
            
            post.CountViews += 1;
            var result = await _repositoryManager.PostRepository.Update(post);           
            var postDto = post.Adapt<PostDto>();
            if (!string.IsNullOrEmpty(post.Images))
            {
                postDto.Images = post.Images.Split(',').ToList();
            }
            return postDto;

        }
        public async Task<PostDto> GetByIdUpdate(int id)
        {
            var post = await _repositoryManager.PostRepository.GetById(id);
            if (post == null) return new PostDto();
            var result = await _repositoryManager.PostRepository.Update(post);
            var postDto = post.Adapt<PostDto>();
            if (!string.IsNullOrEmpty(post.Images))
            {
                postDto.Images = post.Images.Split(',').ToList();
            }
            return postDto;

        }

        public async Task<Post> GetLatestPost()
        {
            return await _repositoryManager.PostRepository.GetLatestPost();
        }

        public async Task<List<PostDto>> Search(PostSearchDto search)
        {
            var result = await _repositoryManager.PostRepository.Search(search);
            var resultDto = result.Adapt<List<PostDto>>();
			if (search.PriceRange.HasValue)
			{
				resultDto = FilterByPriceRange(resultDto, (PriceRange)search.PriceRange.Value);
			}
			if (search.AcreageRange.HasValue)
			{
				resultDto = FilterByAcreageRange(resultDto, (AcreageRange)search.AcreageRange.Value);
			}
			return await FilterData(resultDto);
        }

        public async Task<bool> Update(PostUpdateDto post)
        {
            var postInfo = post.Adapt<Post>();
            postInfo.ModifiedDate = DateTime.Now;
            postInfo.Images = string.Join(",", post.Images);
            var result = await _repositoryManager.PostRepository.Update(postInfo);
            return result;
        }
        public async Task<List<PostDto>> FilterData(List<PostDto> lstPosts)
        {
            if (lstPosts?.Count > 0)
            {
                var userIdLst = lstPosts.Where(x => x.AccountId.HasValue).Select(x => x.AccountId.GetValueOrDefault()).ToList();
                if (userIdLst.Count > 0)
                {
                    var searchUser = new UserSearchDto()
                    {
                        IdLst = userIdLst
                    };
                    var users = (await _repositoryManager.UserRepository.Search(searchUser))?.ToDictionary(x => x.Id, x => x.FullName);
                    if (users?.Count > 0)
                    {
                        foreach (var item in lstPosts)
                        {
                            if (item.AccountId.HasValue && users.ContainsKey(item.AccountId.Value))
                            {
                                item.AccountName = users[item.AccountId.Value];
                            }
                        }
                    }
                }
                var hostelTypeIdLst = lstPosts.Where(x => x.HostelTypeId.HasValue).Select(x => x.HostelTypeId.GetValueOrDefault()).ToList();
                if (hostelTypeIdLst.Count > 0)
                {
                    var searchHostelType = new HostelTypeSearchDto()
                    {
                        IdLst = hostelTypeIdLst
                    };
                    var hostelTypes = (await _repositoryManager.HostelTypeRepository.Search(searchHostelType))?.ToDictionary(x => x.Id, x => x.HostelTypeName);
                    if (hostelTypes?.Count > 0)
                    {
                        foreach (var item in lstPosts)
                        {
                            if (item.HostelTypeId.HasValue && hostelTypes.ContainsKey(item.HostelTypeId.Value))
                            {
                                item.HostelTypeName = hostelTypes[item.HostelTypeId.Value];
                            }
                        }
                    }
                }
            }
            return lstPosts;
        }
        private  List<PostDto> FilterByPriceRange(List<PostDto> lstPosts, PriceRange priceRange)
        {
			return priceRange switch
			{
				PriceRange.AGREEMENT => lstPosts.Where(post => post.Price == null).ToList(),
				PriceRange.BELOW_ONE => lstPosts.Where(post =>  post.Price < 1000000).ToList(),
				PriceRange.ONE_TO_TWO => lstPosts.Where(post => post.Price > 1000000 && post.Price < 2000000).ToList(),
				PriceRange.TWO_TO_FOUR => lstPosts.Where(post => post.Price > 2000000 && post.Price < 4000000).ToList(),
				PriceRange.FOUR_TO_SIX => lstPosts.Where(post => post.Price > 4000000 && post.Price < 6000000).ToList(),
				PriceRange.SIX_TO_EIGHT => lstPosts.Where(post => post.Price > 6000000 && post.Price < 8000000).ToList(),
				PriceRange.EIGHT_TO_TEN => lstPosts.Where(post => post.Price > 8000000 && post.Price < 10000000).ToList(),
				PriceRange.ABOVE_TEN => lstPosts.Where(post => post.Price >= 10000000).ToList(),
				_ => lstPosts,
			};
		}
		private List<PostDto> FilterByAcreageRange(List<PostDto> lstPosts, AcreageRange acreageRange)
		{
			return acreageRange switch
			{
				AcreageRange.BELOW_TWENTY => lstPosts.Where(post =>  post.Acreage < 20).ToList(),
				AcreageRange.TWENTY_TO_FORTY => lstPosts.Where(post =>  post.Acreage >= 20 && post.Acreage < 40).ToList(),
				AcreageRange.FORTY_TO_SIXTY => lstPosts.Where(post => post.Acreage >= 40 && post.Acreage < 60).ToList(),
				AcreageRange.SIXTY_TO_EIGHTY => lstPosts.Where(post => post.Acreage >= 60 && post.Acreage < 80).ToList(),
				AcreageRange.EIGHTY_TO_HUNDRED => lstPosts.Where(post =>  post.Acreage >= 80 && post.Acreage < 100).ToList(),
				AcreageRange.ABOVE_HUNDRED => lstPosts.Where(post =>  post.Acreage >= 100).ToList(),
				_ => lstPosts,
			};
		}
	}
}
