namespace Application.Common.Models
{
	public interface IFile
	{
		Guid UniqueKey { get; }
		string FileName { get; }
		long SizeInBytes { get; }
		Stream OpenReadStream();
	}
}
