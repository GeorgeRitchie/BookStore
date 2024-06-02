using Application.Books.Commands.CreateBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Commands.UpdateBookCategories;
using Application.Books.Queries.GetBook;
using Application.Books.Queries.GetBooks;
using Application.Common.Models;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Extensions;
using WebApi.Models.RequestModels.BookRequestModels;

namespace WebApi.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/{v:apiVersion}/[controller]/[action]")]
	public class BookController : BaseController
	{
		[HttpPost]
		public async Task<IActionResult> Create([FromForm] CreateBookDto bookDto)
		{
			var command = Mapper.Map<CreateBookCommand>(bookDto);
			var result = await Mediator.Send(command);

			return result.To200Or400Or500Response();
		}

		[HttpGet]
		public async Task<Application.Books.Queries.GetBook.BookDto> Get([FromQuery] GetBookQuery query)
		{
			return await Mediator.Send(query);
		}

		[HttpGet]
		public async Task<PaginatedList<Application.Books.Queries.GetBooks.BookDto>> GetAll([FromQuery] GetBooksQuery query)
		{
			return await Mediator.Send(query);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromForm] UpdateBookDto bookDto)
		{
			var command = Mapper.Map<UpdateBookCommand>(bookDto);
			var result = await Mediator.Send(command);

			return result.To200Or400Or500Response();
		}

		[HttpPut]
		public async Task<IActionResult> UpdateBookCategory([FromBody] UpdateBookCategoriesCommand command)
		{
			var result = await Mediator.Send(command);

			return result.To200Or400Or500Response();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete([FromQuery] DeleteBookCommand command)
		{
			var result = await Mediator.Send(command);

			return result.To200Or400Or500Response();
		}
	}
}
