using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.Models.Comment;
using HostelBanking.Entities.Models.HostelType;

namespace HostelBanking.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<bool> Create(Comment comment);
        Task<List<Comment>> GetAll();
        Task<bool> Update(Comment comment);
        Task<bool> Delete(int id);
        Task<Comment> GetById(int id);
        Task<List<Comment>> Search(CommentSearchDto search);
    }
}
