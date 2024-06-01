namespace Application.Common.Models
{
	public class PaginatedList<T>
	{
		public List<T> Items { get; }
		public int PageNumber { get; }
		public int TotalPages { get; }
		public int TotalCount { get; }
		public bool HasPreviousPage => PageNumber > 1;
		public bool HasNextPage => PageNumber < TotalPages;

		public PaginatedList(List<T> items, int totalCount, int pageNumber, int pageSize)
		{
			PageNumber = pageNumber;
			TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
			TotalCount = totalCount;
			Items = items;
		}

		private PaginatedList(int totalCount, int totalPages, int pageNumber, List<T> items)
		{
			PageNumber = pageNumber;
			TotalPages = totalPages;
			TotalCount = totalCount;
			Items = items;
		}

		public static PaginatedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var totalCount = source.Count();
			var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

			return new PaginatedList<T>(items, totalCount, pageNumber, pageSize);
		}

		public static PaginatedList<T> Create(IQueryable<T> source, PaginationParams? paginationParams)
		{
			if (paginationParams != null)
				return Create(source, paginationParams.PageNumber, paginationParams.PageSize);
			else
				return Create(source.ToList());
		}

		public static PaginatedList<T> Create(List<T> items)
		{
			return Create(items, items.Count, 1, 1);
		}

		public static PaginatedList<T> Create(List<T> items, int totalCount, int totalPages, int pageNumber)
		{
			return new PaginatedList<T>(totalCount, totalPages, pageNumber, items);
		}

		public static PaginatedList<T> Create<K>(IQueryable<K> source, int pageNumber, int pageSize, Func<List<K>, List<T>> mappingFunc)
		{
			var totalCount = source.Count();
			var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

			return new PaginatedList<T>(mappingFunc(items), totalCount, pageNumber, pageSize);
		}

		public static PaginatedList<T> Create<K>(IQueryable<K> source, PaginationParams? paginationParams, Func<List<K>, List<T>> mappingFunc)
		{
			if (paginationParams != null)
				return Create(source, paginationParams.PageNumber, paginationParams.PageSize, mappingFunc);
			else
				return Create(mappingFunc(source.ToList()));
		}

		public static PaginatedList<T> Create<K>(List<K> items, Func<List<K>, List<T>> mappingFunc)
		{
			return Create(mappingFunc(items));
		}

		public static PaginatedList<T> Create<K>(List<K> items, int totalCount, int totalPages, int pageNumber, Func<List<K>, List<T>> mappingFunc)
		{
			return Create(mappingFunc(items), totalCount, totalPages, pageNumber);
		}
	}
}
