using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Roles;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Entities.Models.Roles;

namespace HostelBanking.Repositories.Interfaces
{
	public interface IRoleRepository
	{
		Task<bool> Create(Roles roles);
		Task<List<Roles>> GetAll();
		Task<bool> Update(Roles roles);
		Task<bool> Delete(int id);
		Task<Roles> GetById(int id);
		Task<List<Roles>> Search(RoleSearchDto search);
	}
}
