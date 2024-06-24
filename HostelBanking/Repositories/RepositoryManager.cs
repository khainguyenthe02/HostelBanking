using HostelBanking.Repositories.Interfaces;

namespace HostelBanking.Repositories
{
	public class RepositoryManager : IRepositoryManager
	{
		private readonly Lazy<IHostelTypeRepository> lazyHostelTypeRepository;
		public RepositoryManager(IConfiguration configuration)
		{
			lazyHostelTypeRepository = new Lazy<IHostelTypeRepository>(() => new HostelRepository(configuration));
		}
		public IHostelTypeRepository HostelTypeRepository => lazyHostelTypeRepository.Value;
	}
}
