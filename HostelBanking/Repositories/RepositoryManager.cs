using HostelBanking.Repositories.Interfaces;

namespace HostelBanking.Repositories
{
	public class RepositoryManager : IRepositoryManager
	{
		private readonly Lazy<IHostelTypeRepository> lazyHostelTypeRepository;
		private readonly Lazy<IRoleRepository> lazyRoleRepository;
		private readonly Lazy<IUserRepository> lazyUserRepository;
		private readonly Lazy<IPostRepository> lazyPostRepository;
		private readonly Lazy<IPayHistoryRepository> lazyPayHistoryRepository;
		private readonly Lazy<ICommentRepository> lazyCommentRepository;
		private readonly Lazy<IFavoriteRepository> lazyFavoriteRepository;
		private readonly Lazy<IReportRepository> lazyReportRepository;
		public RepositoryManager(IConfiguration configuration)
		{
			lazyHostelTypeRepository = new Lazy<IHostelTypeRepository>(() => new HostelTypeRepository(configuration));
			lazyRoleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(configuration));
			lazyUserRepository = new Lazy<IUserRepository>(() => new UserRepository(configuration));
			lazyPostRepository = new Lazy<IPostRepository>(() => new PostRepository(configuration));
			lazyPayHistoryRepository = new Lazy<IPayHistoryRepository>(() => new PayHistoryRepository(configuration));
            lazyCommentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(configuration));
            lazyFavoriteRepository = new Lazy<IFavoriteRepository>(() => new FavoriteRepository(configuration));
			lazyReportRepository = new Lazy<IReportRepository>(() => new ReportRepository(configuration));
		}
		public IHostelTypeRepository HostelTypeRepository => lazyHostelTypeRepository.Value;
		public IRoleRepository RoleRepository => lazyRoleRepository.Value;


		public IUserRepository UserRepository => lazyUserRepository.Value;

		public IPostRepository PostRepository => lazyPostRepository.Value;

        public IPayHistoryRepository PayHistoryRepository => lazyPayHistoryRepository.Value;

        public ICommentRepository CommentRepository => lazyCommentRepository.Value;

        public IFavoriteRepository FavoriteRepository => lazyFavoriteRepository.Value;

		public IReportRepository ReportRepository => lazyReportRepository.Value;
	}
}
