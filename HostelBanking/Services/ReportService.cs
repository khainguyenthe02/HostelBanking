using HostelBanking.Repositories.Interfaces;

namespace HostelBanking.Services
{
	public class ReportService
	{
		private readonly IRepositoryManager _repositoryManager;
		public ReportService(IRepositoryManager repositoryManager)
		{
			this._repositoryManager = repositoryManager;
		}
	}
}
