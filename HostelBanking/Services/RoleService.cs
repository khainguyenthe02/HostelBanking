using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Roles;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Entities.Models.Roles;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;

namespace HostelBanking.Services
{
	public class RoleService : IRoleService
	{
		private readonly IRepositoryManager _repositoryManager;
		public RoleService(IRepositoryManager repositoryManager)
		{
			this._repositoryManager = repositoryManager;
		}
		public async Task<bool> Create(RoleCreateDto role)
		{
			var roleInfo = role.Adapt<Roles>();
			var result = await _repositoryManager.RoleRepository.Create(roleInfo);
			return result;
		}

		public async Task<bool> Delete(int id)
		{
			RoleSearchDto search = new()
			{
				Id = id,
			};
			var roleInfo = await _repositoryManager.RoleRepository.Search(search);
			if (roleInfo != null)
			{
				var roleUpdate = new Roles();
				roleUpdate.Id = id;
				roleUpdate.DeleteFlag = true;
				var result = await _repositoryManager.RoleRepository.Update(roleUpdate);
				return true;
			}
			return false;
		}

		public async Task<List<RolesDto>> GetAll()
		{
			var result = await _repositoryManager.RoleRepository.GetAll();
			return result?.Adapt<List<RolesDto>>();
		}

		public async Task<RolesDto> GetById(int id)
		{
			var result = await _repositoryManager.RoleRepository.GetById(id);
			return result?.Adapt<RolesDto>();
		}

		public async Task<List<RolesDto>> Search(RoleSearchDto search)
		{
			var result = await _repositoryManager.RoleRepository.Search(search);
			return result?.Adapt<List<RolesDto>>();
		}

		public async Task<bool> Update(RoleUpdateDto roles)
		{
			var roleInfo = roles.Adapt<Roles>();
			var result = await _repositoryManager.RoleRepository.Update(roleInfo);
			return result;
		}
	}
}
