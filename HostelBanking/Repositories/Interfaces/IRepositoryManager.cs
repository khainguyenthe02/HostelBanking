namespace HostelBanking.Repositories.Interfaces
{
	public interface IRepositoryManager
	{
		IHostelTypeRepository HostelTypeRepository { get; }
		IRoleRepository RoleRepository { get; }
	}
}
