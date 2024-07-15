using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.HostelType;

namespace HostelBanking.Services.Interfaces
{
    public interface ICommentService
    {
        Task<bool> Create(CommentCreateDto comment);
        Task<List<CommentDto>> GetAll();
        Task<bool> Delete(int id);
        Task<CommentDto> GetById(int id);
        Task<bool> Update(CommentUpdateDto archives);
        Task<List<CommentDto>> Search(CommentSearchDto search);
    }
}
