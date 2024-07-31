using HostelBanking.Entities.Models;

namespace HostelBanking.Services.Interfaces
{
	public interface IDiscountService
	{
		Task<Discount> LoadFromFile();
		Task<bool> SaveToFile(Discount discount);
	}
}
