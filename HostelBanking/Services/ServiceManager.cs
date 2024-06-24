using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;

namespace HostelBanking.Services
{
	public class ServiceManager : IServiceManager
	{
		private readonly Lazy<IHostelTypeService> lazyHostelTypeService;
		public ServiceManager (IRepositoryManager repositoryManager, IConfiguration configuration,
								IWebHostEnvironment webHostEnvironment)
		{
			lazyHostelTypeService = new Lazy<IHostelTypeService>(() => new HostelTypeService(repositoryManager));
		}
		public IHostelTypeService HostelTypeService => lazyHostelTypeService.Value;
	}
}
