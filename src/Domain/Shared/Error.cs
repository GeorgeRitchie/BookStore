namespace Domain.Shared
{
	public class Error : IEquatable<Error>
	{
		public static readonly Error? None = null;
		public static readonly Error Default = new(string.Empty, string.Empty);

		public string Code { get; }
		public string Message { get; }
		public string? Description { get; } = null;
		public string? Source { get; } = null;
		public Error? InnerError { get; } = null;

		public Error(string code, string message)
		{
			Code = code;
			Message = message;
		}

		public Error(string code, string message, string? description = null, string? source = null, Error? innerError = null) : this(code, message)
		{
			Description = description;
			Source = source;
			InnerError = innerError;
		}

		public static bool operator ==(Error? first, Error? second)
		{
			if (first is null && second is null)
				return true;

			if (first is null || second is null)
				return false;

			return first.Equals(second);
		}

		public static bool operator !=(Error? first, Error? second)
		{
			return !(first == second);
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as Error);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (Code != null) ? Code.GetHashCode() : 0;
				hashCode = (hashCode * 397) ^ ((Message != null) ? Message.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ ((Description != null) ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ ((Source != null) ? Source.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ ((InnerError != null) ? InnerError.GetHashCode() : 0);
				return hashCode;
			}
		}

		public bool Equals(Error? other)
		{
			if (other is null)
				return false;

			if (other.GetType() != GetType())
				return false;

			if (other.Code != Code)
				return false;

			if (other.Message != Message)
				return false;

			if (other.Description != Description)
				return false;

			if (other.Source != Source)
				return false;

			if (other.InnerError != InnerError)
				return false;

			return true;
		}
	}
}
