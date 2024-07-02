using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Roles;

namespace HostelBanking.Services.Interfaces
{
	public interface IRoleService
	{
		Task<bool> Create(RoleCreateDto role);
		Task<List<RolesDto>> GetAll();
		Task<bool> Delete(int id);
		Task<RolesDto> GetById(int id);
		Task<bool> Update(RoleUpdateDto roles);
		Task<List<RolesDto>> Search(RoleSearchDto search);
	}
}
