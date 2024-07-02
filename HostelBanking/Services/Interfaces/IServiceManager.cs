namespace HostelBanking.Services.Interfaces
{
	public interface IServiceManager
	{
		IHostelTypeService HostelTypeService { get; }
		IRoleService RoleService { get; }
	}
}
