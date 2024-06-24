using HostelBanking.Entities.DataTransferObjects;
using HostelBanking.Entities.Models.HostelType;

namespace HostelBanking.Repositories.Interfaces
{
	public interface IHostelTypeRepository
	{
		Task<bool> Create(HostelType hostelType);
		Task<List<HostelType>> GetAll();
		Task<HostelType> Update(HostelType hostelType);
		Task<bool> Delete(string id);
		Task<HostelType> GetById(int id);
		Task<List<HostelType>> Search(HostelTypeSearchDto search);
	}
}
