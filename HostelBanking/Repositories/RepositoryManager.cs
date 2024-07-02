using HostelBanking.Repositories.Interfaces;

namespace HostelBanking.Repositories
{
	public class RepositoryManager : IRepositoryManager
	{
		private readonly Lazy<IHostelTypeRepository> lazyHostelTypeRepository;
		private readonly Lazy<IRoleRepository> lazyRoleRepository;
		public RepositoryManager(IConfiguration configuration)
		{
			lazyHostelTypeRepository = new Lazy<IHostelTypeRepository>(() => new HostelTypeRepository(configuration));
			lazyRoleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(configuration));
		}
		public IHostelTypeRepository HostelTypeRepository => lazyHostelTypeRepository.Value;

		public IRoleRepository RoleRepository => lazyRoleRepository();
	}
}
