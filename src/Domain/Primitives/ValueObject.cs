namespace Domain.Primitives
{
	public abstract class ValueObject : IEquatable<ValueObject>
	{
		public abstract IEnumerable<object> GetAtomicValues();

		public static bool operator ==(ValueObject? first, ValueObject? second)
		{
			if (first is null && second is null)
				return true;

			if (first is null || second is null)
				return false;

			return first.Equals(second);
		}

		public static bool operator !=(ValueObject? first, ValueObject? second)
		{
			return !(first == second);
		}

		public override bool Equals(object? obj)
		{
			return obj is ValueObject other && ValuesAreEqual(other);
		}

		public override int GetHashCode()
		{
			return GetAtomicValues().Aggregate(default(int), HashCode.Combine);
		}

		public bool Equals(ValueObject? other)
		{
			return other is not null && ValuesAreEqual(other);
		}

		private bool ValuesAreEqual(ValueObject other) => GetAtomicValues().SequenceEqual(other.GetAtomicValues());
	}
}
