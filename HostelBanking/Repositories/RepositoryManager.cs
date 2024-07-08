using HostelBanking.Repositories.Interfaces;

namespace HostelBanking.Repositories
{
	public class RepositoryManager : IRepositoryManager
	{
		private readonly Lazy<IHostelTypeRepository> lazyHostelTypeRepository;
		private readonly Lazy<IRoleRepository> lazyRoleRepository;
		private readonly Lazy<IUserRepository> lazyUserRepository;
		private readonly Lazy<IPostRepository> lazyPostRepository;
		private readonly Lazy<IPostImageRepository> lazyPostImageRepository;
		public RepositoryManager(IConfiguration configuration)
		{
			lazyHostelTypeRepository = new Lazy<IHostelTypeRepository>(() => new HostelTypeRepository(configuration));
			lazyRoleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(configuration));
			lazyUserRepository = new Lazy<IUserRepository>(() => new UserRepository(configuration));
			lazyPostRepository = new Lazy<IPostRepository>(() => new PostRepository(configuration));
			lazyPostImageRepository = new Lazy<IPostImageRepository>(() => new PostImageRepository(configuration));
		}
		public IHostelTypeRepository HostelTypeRepository => lazyHostelTypeRepository.Value;
		public IRoleRepository RoleRepository => lazyRoleRepository.Value;


		public IUserRepository UserRepository => lazyUserRepository.Value;

		public IPostRepository PostRepository => lazyPostRepository.Value;

		public IPostImageRepository PostImageRepository => lazyPostImageRepository.Value;
	}
}
