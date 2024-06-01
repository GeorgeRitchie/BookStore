using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[ApiController]
	public abstract class BaseController : ControllerBase
	{
		private IMediator mediator;
		protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

		private IMapper mapper;
		protected IMapper Mapper => mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();
	}
}
