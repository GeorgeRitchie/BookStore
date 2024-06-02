using Application.Categories.Commands.CreateCategory;
using AutoMapper;

namespace WebApi.Models.RequestModels.CategoryRequestModels
{
	public class CreateCategoryDto : IMapWith<CreateCategoryCommand>
	{
		public string Name { get; set; }
		public IFormFile? Icon { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<CreateCategoryDto, CreateCategoryCommand>()
				.ForMember(cmd => cmd.Icon,
							opt => opt.MapFrom(dto => dto.Icon != null ? new FormFileWrapper(dto.Icon) : null));
		}
	}
}
