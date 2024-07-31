namespace HostelBanking.Services.Interfaces
{
	public interface IServiceManager
	{
		IHostelTypeService HostelTypeService { get; }
		IRoleService RoleService { get; }
		IUserService UserService { get; }
		IPostService PostService { get; }
		IPayHistoryService PayHistoryService { get; }
		ICommentService CommentService { get; }
		IFavoriteService FavoriteService { get; }
		IReportService ReportService { get; }
	}
}
