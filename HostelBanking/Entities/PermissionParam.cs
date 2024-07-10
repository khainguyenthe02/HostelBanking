using HostelBanking.Entities.DataTransferObjects.Account;

namespace HostelBanking.Entities
{
	public class PermissionParam
	{
		public OptionalParam<PageParametersDto> Paging { get; set; } = null;
		public UserDto User { get; set; } = null;
		public string CaseLogin { get; set; } = null;
		public string SortBy { get; set; } = null;
		public List<string> SortByLst { get; set; } = null;
		public CancellationToken CancellationToken { get; set; } = default;
	}
}
