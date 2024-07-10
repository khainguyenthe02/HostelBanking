namespace HostelBanking.Entities
{
	public class PageParametersDto
	{
		public PageParametersDto()
		{

		}
		public PageParametersDto(int totalItems, int currentPage = 1, int pageSize = 10, int maxPages = 10)
		{
			if (pageSize < 1)
			{
				pageSize = 10;
			}
			// calculate total pages
			var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);

			// ensure current page isn't out of range
			if (currentPage < 1)
			{
				currentPage = 1;
			}
			else if (currentPage > totalPages)
			{
				currentPage = totalPages;
			}

			int startPage, endPage;
			if (totalPages <= maxPages)
			{
				// total pages less than max so show all pages
				startPage = 1;
				endPage = totalPages;
			}
			else
			{
				// total pages more than max so calculate start and end pages
				var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)maxPages / (decimal)2);
				var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)maxPages / (decimal)2) - 1;
				if (currentPage <= maxPagesBeforeCurrentPage)
				{
					// current page near the start
					startPage = 1;
					endPage = maxPages;
				}
				else if (currentPage + maxPagesAfterCurrentPage >= totalPages)
				{
					// current page near the end
					startPage = totalPages - maxPages + 1;
					endPage = totalPages;
				}
				else
				{
					// current page somewhere in the middle
					startPage = currentPage - maxPagesBeforeCurrentPage;
					endPage = currentPage + maxPagesAfterCurrentPage;
				}
			}

			// calculate start and end item indexes
			var startIndex = (currentPage - 1) * pageSize;
			var endIndex = Math.Min(startIndex + pageSize - 1, totalItems - 1);

			// create an array of pages that can be looped over
			var pages = Enumerable.Range(startPage, (endPage + 1) - startPage);

			// update object instance with all pager properties required by the view
			TotalItems = totalItems;
			CurrentPage = currentPage;
			PageSize = pageSize;
			TotalPages = totalPages;
			StartPage = startPage;
			EndPage = endPage;
			StartIndex = startIndex;
			EndIndex = endIndex;
			Pages = pages;
		}
		public bool Paging { get; set; } = false;
		public int TotalItems { get; set; }
		public int CurrentPage { get; set; } = 1;
		public int PageSize { get; set; } = 20;
		public int TotalPages { get; set; }
		public int StartPage { get; private set; }
		public int EndPage { get; private set; }
		public int StartIndex { get; private set; }
		public int EndIndex { get; private set; }
		public IEnumerable<int> Pages { get; private set; }
		public string SortBy { get; set; }
		public List<string> SortByLst { get; set; }
	}

	public class OptionalParam<Type>
	{
		public Type Value { get; set; }
	}

}
