using Application.Books.Commands.CreateBook;
using AutoMapper;

namespace WebApi.Models.RequestModels.BookRequestModels
{
	public class CreateBookDto : IMapWith<CreateBookCommand>
	{
		public string Title { get; set; }
		public IFormFile? Image { get; set; }
		public string? Description { get; set; }
		public string? ISBN { get; set; }
		public decimal Price { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<CreateBookDto, CreateBookCommand>()
				.ForMember(cmd => cmd.Image,
							opt => opt.MapFrom(dto => dto.Image != null ? new FormFileWrapper(dto.Image) : null));
		}
	}
}
