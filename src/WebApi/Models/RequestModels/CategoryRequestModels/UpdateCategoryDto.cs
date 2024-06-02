using Application.Categories.Commands.UpdateCategory;
using AutoMapper;

namespace WebApi.Models.RequestModels.CategoryRequestModels
{
	public class UpdateCategoryDto : IMapWith<UpdateCategoryCommand>
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public IFormFile? Icon { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<UpdateCategoryDto, UpdateCategoryCommand>()
				.ForMember(cmd => cmd.Icon,
							opt => opt.MapFrom(dto => dto.Icon != null ? new FormFileWrapper(dto.Icon) : null));
		}
	}
}
