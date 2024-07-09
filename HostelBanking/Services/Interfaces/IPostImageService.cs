using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.PostImage;

namespace HostelBanking.Services.Interfaces
{
	public interface IPostImageService
	{
        Task<bool> Create(PostImageCreateDto postImage);
        Task<bool> CreateList(List<PostImageDto> list);
        Task<List<PostImageDto>> GetAll();
        Task<bool> Delete(int id);
        Task<PostImageDto> GetById(int id);
        Task<bool> Update(PostImageUpdateDto archives);
        Task<List<PostImageDto>> Search(PostImageSearchDto search);
    }
}
