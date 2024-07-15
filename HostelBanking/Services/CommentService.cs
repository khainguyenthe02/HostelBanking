using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.Models.Comment;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;

namespace HostelBanking.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepositoryManager _repositoryManager;
        public CommentService(IRepositoryManager repositoryManager)
        {
            this._repositoryManager = repositoryManager;
        }

        public async Task<bool> Create(CommentCreateDto comment)
        {
            var hostelTypeInfo = comment.Adapt<Comment>();
            var result = await _repositoryManager.CommentRepository.Create(hostelTypeInfo);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            CommentSearchDto search = new()
            {
                Id = id,
            };
            var hostelTypeInfo = await _repositoryManager.CommentRepository.Search(search);
            if (hostelTypeInfo != null)
            {
                var hostelTypeUpdate = new Comment();
                hostelTypeUpdate.Id = id;
                hostelTypeUpdate.DeleteFlag = true;
                var result = await _repositoryManager.CommentRepository.Update(hostelTypeUpdate);
                return true;
            }
            return false;
        }

        public async Task<List<CommentDto>> GetAll()
        {
            var result = await _repositoryManager.CommentRepository.GetAll();
            return result.Adapt<List<CommentDto>>();
        }

        public async Task<CommentDto> GetById(int id)
        {
            var result = await _repositoryManager.CommentRepository.GetById(id);
            return result.Adapt<CommentDto>();
        }

        public async Task<List<CommentDto>> Search(CommentSearchDto search)
        {
            var result = await _repositoryManager.CommentRepository.Search(search);
            return result.Adapt<List<CommentDto>>();
        }

        public async Task<bool> Update(CommentUpdateDto comment)
        {
            var hostelTypeInfo = comment.Adapt<Comment>();
            var result = await _repositoryManager.CommentRepository.Update(hostelTypeInfo);
            return result;
        }
    }
}
