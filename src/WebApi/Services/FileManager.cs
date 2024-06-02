using Application.Common.Enumerations;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Domain.Shared;
using WebApi.Common.Errors;

namespace WebApi.Services
{
	public class FileManager(ILogger<FileManager> logger, IWebHostEnvironment environment) : IFileManager
	{
		private readonly HashSet<string> _validImageExtensions = new(StringComparer.OrdinalIgnoreCase)
		{
			".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".ico",
			".svg",
			".webp"
		};

		public Result Delete(string? fileAbsolutePath)
		{
			if (string.IsNullOrWhiteSpace(fileAbsolutePath) || File.Exists(fileAbsolutePath) == false)
				return Result.Success();

			try
			{
				File.Delete(fileAbsolutePath);
				return Result.Success();
			}
			catch (Exception ex)
			{
				logger.LogDebug("{source} - Something went wrong while removing file with path {path}. Ex: {@ex}",
												nameof(FileManager),
												fileAbsolutePath, ex);

				return Result.Failure(WebApiErrors.Common.OperationFailed(ex.Message));
			}
		}

		public Result Delete(string? fileRelativePath, ImageType imageType)
		{
			ArgumentNullException.ThrowIfNull(imageType);

			if (string.IsNullOrWhiteSpace(fileRelativePath))
				return Result.Success();

			return Delete(Path.Combine(environment.WebRootPath, imageType.Value, fileRelativePath));
		}

		public string GetAbsolutePath(IFile file, ImageType imageType)
		{
			ArgumentNullException.ThrowIfNull(file);
			ArgumentNullException.ThrowIfNull(imageType);

			return Path.Combine(environment.WebRootPath, imageType.Value, file.FileName);
		}

		public string GetRelativePath(IFile file, ImageType imageType)
		{
			ArgumentNullException.ThrowIfNull(file);
			ArgumentNullException.ThrowIfNull(imageType);

			return Path.Combine(imageType.Value, file.FileName);
		}

		public bool IsPhoto(IFile file)
		{
			if (file == null || string.IsNullOrWhiteSpace(file.FileName))
				return false;

			string extension = Path.GetExtension(file.FileName);
			return _validImageExtensions.Contains(extension);
		}

		public Stream OpenReadOnlyStream(string fileAbsolutePath)
		{
			return new FileStream(fileAbsolutePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		public Stream OpenReadOnlyStream(string fileRelativePath, ImageType imageType)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(fileRelativePath);
			ArgumentNullException.ThrowIfNull(imageType);

			return OpenReadOnlyStream(Path.Combine(environment.WebRootPath));
		}

		public async Task<string> SaveAsync(IFile file, ImageType imageType, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(file);
			ArgumentNullException.ThrowIfNull(imageType);

			var absolutePath = GetAbsolutePath(file, imageType);

			var directoryPath = Path.GetDirectoryName(absolutePath);
			if (Directory.Exists(directoryPath) == false)
				Directory.CreateDirectory(directoryPath);

			using (var fileStream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write))
			{
				await file.OpenReadStream().CopyToAsync(fileStream, cancellationToken);
			}

			return absolutePath;
		}
	}
}
