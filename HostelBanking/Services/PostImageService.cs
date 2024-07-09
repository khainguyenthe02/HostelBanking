using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.PostImage;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Entities.Models.PostImages;
using HostelBanking.Repositories;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;

namespace HostelBanking.Services
{
    public class PostImageService : IPostImageService
    {
        private readonly IRepositoryManager _repositoryManager;
        public PostImageService(IRepositoryManager repositoryManager)
        {
            this._repositoryManager = repositoryManager;
        }

        public async Task<bool> Create(PostImageCreateDto postImage)
        {
            var postImageInfo = postImage.Adapt<PostImage>();
            var result = await _repositoryManager.PostImageRepository.Create(postImageInfo);
            return result;
        }

        public async Task<bool> CreateList(List<PostImageDto> list)
        {
            if (list.Count > 0)
            {
                foreach (var postImageDto in list)
                {
                    var postImage = postImageDto.Adapt<PostImage>();
                    var result = await _repositoryManager.PostImageRepository.Create(postImage);
                    if (!result)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            PostImageSearchDto search = new()
            {
                Id = id,
            };
            var postImageInfo = await _repositoryManager.PostImageRepository.Search(search);
            if (postImageInfo != null)
            {
                var postImageUpdate = new PostImage();
                postImageUpdate.Id = id;
                postImageUpdate.DeleteFlag = true;
                var result = await _repositoryManager.PostImageRepository.Update(postImageUpdate);
                return true;
            }
            return false;
        }

        public async Task<List<PostImageDto>> GetAll()
        {
            var result = await _repositoryManager.PostImageRepository.GetAll();
            return result.Adapt<List<PostImageDto>>();
        }

        public async Task<PostImageDto> GetById(int id)
        {
            var result = await _repositoryManager.PostImageRepository.GetById(id);
            return result.Adapt<PostImageDto>();
        }

        public async Task<List<PostImageDto>> Search(PostImageSearchDto search)
        {
            var result = await _repositoryManager.PostImageRepository.Search(search);
            return result.Adapt<List<PostImageDto>>();
        }

        public async Task<bool> Update(PostImageUpdateDto postImage)
        {
            var postImageInfo = postImage.Adapt<PostImage>();
            var result = await _repositoryManager.PostImageRepository.Update(postImageInfo);
            return result;
        }
    }
}
