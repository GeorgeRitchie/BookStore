using Application.Common.Enumerations;
using Application.Common.Models;

namespace Application.Common.Interfaces.Services
{
	public interface IFileManager
	{
		bool IsPhoto(IFile file);
		string GetRelativePath(IFile file, ImageType imageType);
		string GetAbsolutePath(IFile file, ImageType imageType);
		Task<string> SaveAsync(IFile file, ImageType imageType, CancellationToken cancellationToken = default);
		Stream OpenReadOnlyStream(string fileAbsolutePath);
		Stream OpenReadOnlyStream(string fileRelativePath, ImageType imageType);
		Result Delete(string? fileAbsolutePath);
		Result Delete(string fileRelativePath, ImageType imageType);
	}
}
