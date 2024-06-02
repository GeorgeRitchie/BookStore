using Domain.Primitives;

namespace Application.Common.Enumerations
{
	public sealed class ImageType : Enumeration<ImageType, string>
	{
		public static readonly ImageType BookImage = new("book-image", "images");
		public static readonly ImageType CategoryIcon = new("category-icon", "icons");
		public static readonly ImageType AuthorPhoto = new("author-photo", "photos");
		public static readonly ImageType UserPhoto = new("user-photo", "photos");

		private ImageType(string name, string value) : base(name, value)
		{
		}

		protected override bool IsValueEqual(string? first, string? second)
		{
			return first == second;
		}
	}
}
