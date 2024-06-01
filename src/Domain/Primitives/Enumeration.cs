using System.Reflection;

namespace Domain.Primitives
{
	public abstract class Enumeration<TEnum, TEnumValue> : IEquatable<Enumeration<TEnum, TEnumValue>> where TEnum : Enumeration<TEnum, TEnumValue>
	{
		private static readonly Dictionary<string, TEnum> Enumerations = CreateEnumerations();

		public string Name { get; }

		public TEnumValue Value { get; }

		protected Enumeration(string name, TEnumValue value)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			if (value == null)
				throw new ArgumentNullException(nameof(value));

			Name = name;
			Value = value;
		}

		protected abstract bool IsValueEqual(TEnumValue? first, TEnumValue? second);

		public static bool operator ==(Enumeration<TEnum, TEnumValue>? first, Enumeration<TEnum, TEnumValue>? second)
		{
			if (first is null && second is null)
				return true;

			if (first is null || second is null)
				return false;

			return first.Equals(second);
		}

		public static bool operator !=(Enumeration<TEnum, TEnumValue>? first, Enumeration<TEnum, TEnumValue>? second)
		{
			return !(first == second);
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as Enumeration<TEnum, TEnumValue>);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name.GetHashCode(), Value?.GetHashCode() ?? 0);
		}

		public override string ToString()
		{
			return Name;
		}

		public bool Equals(Enumeration<TEnum, TEnumValue>? other)
		{
			if (other is null)
				return false;

			if (other.GetType() != GetType())
				return false;

			return other.Name == Name && IsValueEqual(other.Value, Value);
		}

		public static TEnum? FromName(string name)
		{
			return Enumerations.TryGetValue(name, out TEnum? enumeration) ? enumeration : default;
		}

		public static TEnum? FromValue(TEnumValue value)
		{
			return Enumerations.Values.SingleOrDefault(e => e.IsValueEqual(e.Value, value));
		}

		public static bool Contains(string name) => Enumerations.Keys.Contains(name);

		public static IReadOnlyCollection<TEnum> GetValues() => Enumerations.Values.ToList();

		private static Dictionary<string, TEnum> CreateEnumerations()
		{
			var enumerationType = typeof(TEnum);

			var fieldsFromType = enumerationType.GetFields(BindingFlags.Public
												 | BindingFlags.Static
												 | BindingFlags.FlattenHierarchy)
									  .Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
									  .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);

			return fieldsFromType.ToDictionary(x => x.Name);
		}
	}

	public abstract class Enumeration<TEnum> : Enumeration<TEnum, int> where TEnum : Enumeration<TEnum>
	{
		protected Enumeration(string name, int value) : base(name, value)
		{
		}

		protected sealed override bool IsValueEqual(int first, int second)
		{
			return first == second;
		}
	}
}
