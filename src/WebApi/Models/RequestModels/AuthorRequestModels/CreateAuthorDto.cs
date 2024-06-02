using Application.Authors.Commands.CreateAuthor;
using AutoMapper;

namespace WebApi.Models.RequestModels.AuthorRequestModels
{
	public class CreateAuthorDto : IMapWith<CreateAuthorCommand>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public IFormFile? Photo { get; set; }
		public string? Description { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<CreateAuthorDto, CreateAuthorCommand>()
				.ForMember(cmd => cmd.Photo,
							opt => opt.MapFrom(dto => dto.Photo != null ? new FormFileWrapper(dto.Photo) : null));
		}
	}
}
