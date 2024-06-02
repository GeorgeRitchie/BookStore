using Application.Common.Models;

namespace Application.Books.Queries.GetBook
{
	public class BookDto : IMapWith<Book>
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string? Image { get; set; }
		public string? Description { get; set; }
		public string? ISBN { get; set; }
		public decimal Price { get; set; }

		public List<EntityMinData> Categories { get; set; }
		public List<EntityMinData> Authors { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<Book, BookDto>()
				.ForMember(dto => dto.Categories,
						   opt => opt.MapFrom(b => b.Categories
															.Select(c => new EntityMinData()
															{
																Id = c.Id,
																Title = c.Name,
															})))
				.ForMember(dto => dto.Authors,
						   opt => opt.MapFrom(b => b.Authors
															.Select(a => new EntityMinData()
															{
																Id = a.Id,
																Title = $"{a.FirstName} {a.LastName}",
															})));
		}
	}
}
