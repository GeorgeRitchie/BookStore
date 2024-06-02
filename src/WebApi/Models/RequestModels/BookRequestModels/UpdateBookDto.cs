using Application.Books.Commands.UpdateBook;
using AutoMapper;

namespace WebApi.Models.RequestModels.BookRequestModels
{
	public class UpdateBookDto : IMapWith<UpdateBookCommand>
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public IFormFile? Image { get; set; }
		public string? Description { get; set; }
		public string? ISBN { get; set; }
		public decimal Price { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<UpdateBookDto, UpdateBookCommand>()
				.ForMember(cmd => cmd.Image,
							opt => opt.MapFrom(dto => dto.Image != null ? new FormFileWrapper(dto.Image) : null));
		}
	}
}
