using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/{v:apiVersion}/[controller]/[action]")]
	public class UserController : BaseController
	{
	}
}
