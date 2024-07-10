namespace HostelBanking.Entities
{
	public class ApiPaginationWrapperDto
	{
		public ApiPaginationWrapperDto()
		{

		}
		public ApiPaginationWrapperDto(PageParametersDto pageParametersDto)
		{
			this.TotalItems = pageParametersDto.TotalItems;
			this.CurrentPage = pageParametersDto.CurrentPage;
			this.PageSize = pageParametersDto.PageSize;
			this.TotalPages = pageParametersDto.TotalPages;
		}
		public ApiPaginationWrapperDto(int TotalItems, int CurrentPage, int PageSize, int TotalPages)
		{
			this.TotalItems = TotalItems;
			this.CurrentPage = CurrentPage;
			this.PageSize = PageSize;
			this.TotalPages = TotalPages;
		}
		public List<object> Data { get; set; }
		public int TotalItems { get; set; }
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int TotalPages { get; set; }
		public object ObjectData { get; set; }
	}
}
