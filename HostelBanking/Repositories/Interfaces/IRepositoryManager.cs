namespace HostelBanking.Repositories.Interfaces
{
	public interface IRepositoryManager
	{
		IHostelTypeRepository HostelTypeRepository { get; }
		IRoleRepository RoleRepository { get; }
		IUserRepository UserRepository { get; }
		IPostRepository PostRepository { get; }
		IPostImageRepository PostImageRepository { get; }
		IPayHistoryRepository PayHistoryRepository { get; }
		ICommentRepository CommentRepository { get; }
		IFavoriteRepository FavoriteRepository { get; }
		IReportRepository ReportRepository { get; }
	}
}
