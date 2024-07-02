using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;

namespace HostelBanking.Services
{
	public class ServiceManager : IServiceManager
	{
		private readonly Lazy<IHostelTypeService> lazyHostelTypeService;
		private readonly Lazy<IRoleService> lazyRoleService;
		public ServiceManager (IRepositoryManager repositoryManager, IConfiguration configuration,
								IWebHostEnvironment webHostEnvironment)
		{
			lazyHostelTypeService = new Lazy<IHostelTypeService>(() => new HostelTypeService(repositoryManager));
			lazyRoleService = new Lazy<IRoleService>(() => new RoleService(repositoryManager));

		}
		public IHostelTypeService HostelTypeService => lazyHostelTypeService.Value;

		public IRoleService RoleService => lazyRoleService.Value;
	}
}
