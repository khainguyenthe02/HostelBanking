using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.DataTransferObjects.PostImage;
using HostelBanking.Entities.DataTransferObjects.Roles;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Entities.Models.Post;
using HostelBanking.Entities.Models.PostImages;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
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
            postInfo.CreateDate = DateTime.Now.Date;
            postInfo.ModifiedDate = DateTime.Now.Date;
            postInfo.Images = string.Join(",", post.Images);
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
                var postUpdate = new Post();
                postUpdate.Id = id;
                postUpdate.DeleteFlag = true;
                var result = await _repositoryManager.PostRepository.Update(postUpdate);
                return true;
            }
            return false;
        }

        public async Task<List<PostDto>> GetAll()
        {
            var result = await _repositoryManager.PostRepository.GetAll();
            return result.Adapt<List<PostDto>>();
        }

        public async Task<PostDto> GetById(int id)
        {
            var result = await _repositoryManager.PostRepository.GetById(id);
            var postDto = result.Adapt<PostDto>();
            if (!string.IsNullOrEmpty(result.Images))
            {
                postDto.Images = result.Images.Split(',').ToList();
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
            return await FilterData(resultDto);
        }

        public async Task<bool> Update(PostUpdateDto post)
        {
            var postInfo = post.Adapt<Post>();
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
    }
}
