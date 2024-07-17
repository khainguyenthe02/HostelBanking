using HostelBanking.Entities.DataTransferObjects.Report;
using HostelBanking.Entities.Models.Report;

namespace HostelBanking.Repositories.Interfaces
{
	public interface IReportRepository
	{
		Task<bool> Create(Report report);
		Task<List<Report>> GetAll();
		Task<bool> Update(Report report);
		Task<bool> Delete(int id);
		Task<Report> GetById(int id);
		Task<List<Report>> Search(ReportSearchDto search);
	}
}
