using Application.Categories.Commands.CreateCategory;
using Application.Categories.Commands.DeleteCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Queries.GetCategories;
using Application.Categories.Queries.GetCategory;
using Application.Common.Models;
using Asp.Versioning;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Extensions;
using WebApi.Models.RequestModels.CategoryRequestModels;

namespace WebApi.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/{v:apiVersion}/[controller]/[action]")]
	public class CategoryController : BaseController
	{
		[HttpPost]
		public async Task<IActionResult> Create([FromForm] CreateCategoryDto categoryDto)
		{
			var command = Mapper.Map<CreateCategoryCommand>(categoryDto);
			var result = await Mediator.Send(command);

			return result.To200Or400Or500Response();
		}

		[HttpGet]
		public async Task<Category> Get([FromQuery] GetCategoryQuery query)
		{
			return await Mediator.Send(query);
		}

		[HttpGet]
		public async Task<PaginatedList<Category>> GetAll([FromQuery] GetCategoriesQuery query)
		{
			return await Mediator.Send(query);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromForm] UpdateCategoryDto categoryDto)
		{
			var command = Mapper.Map<UpdateCategoryCommand>(categoryDto);
			var result = await Mediator.Send(command);

			return result.To200Or400Or500Response();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromQuery] DeleteCategoryCommand command)
		{
			var result = await Mediator.Send(command);

			return result.To200Or400Or500Response();
		}
	}
}
