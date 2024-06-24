using HostelBanking.Entities.DataTransferObjects;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;
using System.Security;

namespace HostelBanking.Services
{
	public class HostelTypeService : IHostelTypeService
	{
		private readonly IRepositoryManager repositoryManager;
		public HostelTypeService(IRepositoryManager repositoryManager)
		{
			this.repositoryManager = repositoryManager;
		}

		public async Task<bool> Create(HostelTypeCreateDto hostelType)
		{
			var hostelTypeInfo = hostelType.Adapt<HostelType>();
			var result = await repositoryManager.HostelTypeRepository.Create(hostelTypeInfo);
			return result;
		}

		public async Task<bool> Delete(int id)
		{
			HostelTypeSearchDto search = new()
			{
				Id = id,
			};
			var hostelTypeInfo = await repositoryManager.HostelTypeRepository.Search(search);
			if (hostelTypeInfo != null)
			{
				var hostelTypeUpdate = new HostelType();
				hostelTypeUpdate.Id = id;
				hostelTypeUpdate.DeleteFlag = true;
				var result = await repositoryManager.HostelTypeRepository.Update(hostelTypeUpdate);
				return true;
			}
			return false;
		}

		public async Task<List<HostelTypeDto>> GetAll()
		{
			var result = await repositoryManager.HostelTypeRepository.GetAll();
			return result?.Adapt<List<HostelTypeDto>>();
		}

		public async Task<HostelTypeDto> GetById(int id)
		{
			var result = await repositoryManager.HostelTypeRepository.GetById(id);
			return result?.Adapt<HostelTypeDto>();
		}

		public async Task<List<HostelTypeDto>> Search(HostelTypeSearchDto search)
		{
			var result = await repositoryManager.HostelTypeRepository.Search(search);
			var resutltDto = result?.Adapt<List<HostelTypeDto>>();
			return await FilterData(resutltDto);
		}

		private Task<List<HostelTypeDto>> FilterData(List<HostelTypeDto>? resutltDto)
		{
			throw new NotImplementedException();
		}
	}
}
