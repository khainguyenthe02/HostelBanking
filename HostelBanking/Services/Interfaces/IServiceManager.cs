namespace HostelBanking.Services.Interfaces
{
	public interface IServiceManager
	{
		IHostelTypeService HostelTypeService { get; }
		IRoleService RoleService { get; }
		IUserService UserService { get; }
		IPostService PostService { get; }
		IPostImageService PostImageService { get; }
	}
}
