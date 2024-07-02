using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Repositories;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;
using System.Security;
using System.Security.Permissions;

namespace HostelBanking.Services
{
    public class HostelTypeService : IHostelTypeService
	{
		private readonly IRepositoryManager _repositoryManager;
		public HostelTypeService(IRepositoryManager repositoryManager)
		{
			this._repositoryManager = repositoryManager;
		}

		public async Task<bool> Create(HostelTypeCreateDto hostelType)
		{
			var hostelTypeInfo = hostelType.Adapt<HostelType>();
			var result = await _repositoryManager.HostelTypeRepository.Create(hostelTypeInfo);
			return result;
		}

		public async Task<bool> Delete(int id)
		{
			HostelTypeSearchDto search = new()
			{
				Id = id,
			};
			var hostelTypeInfo = await _repositoryManager.HostelTypeRepository.Search(search);
			if (hostelTypeInfo != null)
			{
				var hostelTypeUpdate = new HostelType();
				hostelTypeUpdate.Id = id;
				hostelTypeUpdate.DeleteFlag = true;
				var result = await _repositoryManager.HostelTypeRepository.Update(hostelTypeUpdate);
				return true;
			}
			return false;
		}

		public async Task<List<HostelTypeDto>> GetAll()
		{
			var result = await _repositoryManager.HostelTypeRepository.GetAll();
			return result?.Adapt<List<HostelTypeDto>>();
		}

		public async Task<HostelTypeDto> GetById(int id)
		{
			var result = await _repositoryManager.HostelTypeRepository.GetById(id);
			return result?.Adapt<HostelTypeDto>();
		}

		public async Task<List<HostelTypeDto>> Search(HostelTypeSearchDto search)
		{
			var result = await _repositoryManager.HostelTypeRepository.Search(search);
			return result?.Adapt<List<HostelTypeDto>>();
			
		}

		public async Task<bool> Update(HostelTypeUpdateDto hostelType)
		{
			var hostelTypeInfo = hostelType.Adapt<HostelType>();
			var result = await _repositoryManager.HostelTypeRepository.Update(hostelTypeInfo);
			return result;
		}

		private Task<List<HostelTypeDto>> FilterData(List<HostelTypeDto>? resutltDto)
		{
			throw new NotImplementedException();
		}
	}
}
