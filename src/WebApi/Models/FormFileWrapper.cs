using Application.Common.Models;

namespace WebApi.Models
{
	public class FormFileWrapper(IFormFile formFile) : IFile
	{
		private readonly IFormFile formFile = formFile ?? throw new ArgumentNullException(nameof(formFile));

		public string FileName => formFile.FileName;

		public long SizeInBytes => formFile.Length;

		public Guid UniqueKey { get; private init; } = Guid.NewGuid();

		public Stream OpenReadStream()
		{
			return formFile.OpenReadStream();
		}
	}
}
