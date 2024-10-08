﻿using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;

namespace HostelBanking.Services
{
    public class ServiceManager : IServiceManager
	{
		private readonly Lazy<IHostelTypeService> lazyHostelTypeService;
		private readonly Lazy<IRoleService> lazyRoleService;
		private readonly Lazy<IUserService> lazyUserService;
		private readonly Lazy<IPostService> lazyPostService;
		private readonly Lazy<IPayHistoryService> lazyPayHistoryService;
		private readonly Lazy<ICommentService> lazyCommentService;
		private readonly Lazy<IFavoriteService> lazyFavoriteService;
		private readonly Lazy<IReportService> lazyReportService;
        public ServiceManager (IRepositoryManager repositoryManager, IConfiguration configuration,
								IWebHostEnvironment webHostEnvironment)
		{
			lazyHostelTypeService = new Lazy<IHostelTypeService>(() => new HostelTypeService(repositoryManager));
			lazyRoleService = new Lazy<IRoleService>(() => new RoleService(repositoryManager));
			lazyUserService = new Lazy<IUserService>(() => new UserService(repositoryManager));
			lazyPostService = new Lazy<IPostService>(() => new PostService(repositoryManager));
            lazyPayHistoryService = new Lazy<IPayHistoryService>(() => new PayHistoryService(repositoryManager));
			lazyCommentService = new Lazy<ICommentService>(()  => new CommentService(repositoryManager));
            lazyFavoriteService = new Lazy<IFavoriteService>(()  => new FavoriteService(repositoryManager));
			lazyReportService = new Lazy<IReportService>(() => new ReportService(repositoryManager));

        }
		public IHostelTypeService HostelTypeService => lazyHostelTypeService.Value;

		public IRoleService RoleService => lazyRoleService.Value;

		public IUserService UserService => lazyUserService.Value;

		public IPostService PostService => lazyPostService.Value;

        public IPayHistoryService PayHistoryService => lazyPayHistoryService.Value;

        public ICommentService CommentService => lazyCommentService.Value;

        public IFavoriteService FavoriteService => lazyFavoriteService.Value;

        public IReportService ReportService => lazyReportService.Value;
    }
}
