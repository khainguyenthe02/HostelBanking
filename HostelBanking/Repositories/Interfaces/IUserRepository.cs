using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.Models.Account;
using HostelBanking.Entities.Models.HostelType;

namespace HostelBanking.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<bool> Create(User user);
		Task<List<User>> GetAll();
		Task<bool> Update(User user);
		Task<bool> Delete(string id);
		Task<User> GetById(int id);
		Task<List<User>> Search(UserSearchDto search);
		Task<User> Login(string email, string password);
	}
}
