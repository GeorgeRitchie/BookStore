using Application.Authors.Commands.CreateAuthor;
using Application.Authors.Commands.DeleteAuthor;
using Application.Authors.Commands.UpdateAuthor;
using Application.Authors.Queries.GetAuthor;
using Application.Authors.Queries.GetAuthors;
using Application.Common.Models;
using Asp.Versioning;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Extensions;
using WebApi.Models.RequestModels.AuthorRequestModels;

namespace WebApi.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/{v:apiVersion}/[controller]/[action]")]
	public class AuthorController : BaseController
	{
		[HttpPost]
		public async Task<IActionResult> Create([FromForm] CreateAuthorDto authorDto)
		{
			var command = Mapper.Map<CreateAuthorCommand>(authorDto);
			var result = await Mediator.Send(command);

			return result.To200Or400Or500Response();
		}

		[HttpGet]
		public async Task<Author> Get([FromQuery] GetAuthorQuery query)
		{
			return await Mediator.Send(query);
		}

		[HttpGet]
		public async Task<PaginatedList<Author>> GetAll([FromQuery] GetAuthorsQuery query)
		{
			return await Mediator.Send(query);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromForm] UpdateAuthorDto authorDto)
		{
			var command = Mapper.Map<UpdateAuthorCommand>(authorDto);
			var result = await Mediator.Send(command);

			return result.To200Or400Or500Response();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromQuery] DeleteAuthorCommand command)
		{
			var result = await Mediator.Send(command);

			return result.To200Or400Or500Response();
		}
	}
}
