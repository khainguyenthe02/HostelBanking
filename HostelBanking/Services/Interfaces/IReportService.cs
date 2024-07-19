using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.Report;

namespace HostelBanking.Services.Interfaces
{
	public interface IReportService
	{
        Task<bool> Create(ReportCreateDto report);
        Task<List<ReportDto>> GetAll();
        Task<bool> Delete(int id);
        Task<ReportDto> GetById(int id);
        Task<bool> Update(ReportUpdateDto report);
        Task<List<ReportDto>> Search(ReportSearchDto search);
    }
}
