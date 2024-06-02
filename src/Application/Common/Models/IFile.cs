namespace Application.Common.Models
{
	public interface IFile
	{
		string FileName { get; }
		long SizeInBytes { get; }
		Stream OpenReadStream();
	}
}
