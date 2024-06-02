using Application.Authors.Commands.UpdateAuthor;
using AutoMapper;

namespace WebApi.Models.RequestModels.AuthorRequestModels
{
	public class UpdateAuthorDto : IMapWith<UpdateAuthorCommand>
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public IFormFile? Photo { get; set; }
		public string? Description { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<UpdateAuthorDto, UpdateAuthorCommand>()
				.ForMember(cmd => cmd.Photo,
							opt => opt.MapFrom(dto => dto.Photo != null ? new FormFileWrapper(dto.Photo) : null));
		}
	}
}
