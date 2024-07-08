using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;

namespace HostelBanking.Services
{
	public class ServiceManager : IServiceManager
	{
		private readonly Lazy<IHostelTypeService> lazyHostelTypeService;
		private readonly Lazy<IRoleService> lazyRoleService;
		private readonly Lazy<IUserService> lazyUserService;
		private readonly Lazy<IPostService> lazyPostService;
		public ServiceManager (IRepositoryManager repositoryManager, IConfiguration configuration,
								IWebHostEnvironment webHostEnvironment)
		{
			lazyHostelTypeService = new Lazy<IHostelTypeService>(() => new HostelTypeService(repositoryManager));
			lazyRoleService = new Lazy<IRoleService>(() => new RoleService(repositoryManager));
			lazyUserService = new Lazy<IUserService>(() => new UserService(repositoryManager));
			lazyPostService = new Lazy<IPostService>(() => new PostService(repositoryManager));

		}
		public IHostelTypeService HostelTypeService => lazyHostelTypeService.Value;

		public IRoleService RoleService => lazyRoleService.Value;

		public IUserService UserService => lazyUserService.Value;

		public IPostService PostService => lazyPostService.Value;
	}
}
