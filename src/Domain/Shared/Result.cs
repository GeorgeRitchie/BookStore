namespace Domain.Shared
{
	public class Result
	{
		protected bool _status;
		protected readonly List<Error> _errors;

		public bool IsSuccess => _status;
		public bool IsFailure => !IsSuccess;
		public IReadOnlyCollection<Error> Errors => _errors;

		protected Result(bool isSuccess, Error? error)
		{
			if (isSuccess == true && error != Error.None)
				throw new ArgumentException($"Inappropriate values of '{nameof(isSuccess)}' and '{nameof(error)}'");

			if (isSuccess == false && (error == null || error == Error.None))
				throw new ArgumentException($"Inappropriate values of '{nameof(isSuccess)}' and '{nameof(error)}'");

			_status = isSuccess;
			_errors = isSuccess == true ? new() : new() { error! };
		}

		protected Result(bool isSuccess, IEnumerable<Error> errors)
		{
			ArgumentNullException.ThrowIfNull(errors, nameof(errors));

			if (isSuccess == true && errors.Any() == true)
				throw new ArgumentException($"Inappropriate values of '{nameof(isSuccess)}' and '{nameof(errors)}'");

			if (isSuccess == false && errors.Any() == false)
				throw new ArgumentException($"Inappropriate values of '{nameof(isSuccess)}' and '{nameof(errors)}'");

			_status = isSuccess;
			_errors = errors.ToList();
		}

		public static Result Success() => new(true, Error.None);
		public static Result Failure() => new(false, Error.Default);
		public static Result Failure(Error error) => new(false, error);
		public static Result Failure(IEnumerable<Error> errors) => new(false, errors);
		public static Result<TValue> Success<TValue>(TValue? value) => Result<TValue>.Success(value);
		public static Result<TValue> Failure<TValue>(TValue? value) => Result<TValue>.Failure(value);
		public static Result<TValue> Failure<TValue>(Error error) => Result<TValue>.Failure(default, error);
		public static Result<TValue> Failure<TValue>(TValue? value, Error error) => Result<TValue>.Failure(value, error);
		public static Result<TValue> Failure<TValue>(TValue? value, IEnumerable<Error> errors) => Result<TValue>.Failure(value, errors);
		public static Result<TValue> Combine<TValue>(params Result<TValue>[] results) => Result<TValue>.Combine(results);

		public Result Ensure(Func<bool> predicate, Error error)
		{
			ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
			ArgumentNullException.ThrowIfNull(error, nameof(error));

			if (predicate.Invoke() == false)
			{
				_status = false;
				_errors.Add(error);
			}

			return this;
		}
	}

	public class Result<TValue> : Result
	{
		private readonly TValue? _value;

		public TValue? Value => IsSuccess == true ? _value : default;

		protected Result(TValue? value, bool isSuccess, Error? error) : base(isSuccess, error)
		{
			_value = value;
		}

		protected Result(TValue? value, bool isSuccess, IEnumerable<Error> errors) : base(isSuccess, errors)
		{
			_value = value;
		}

		public static implicit operator Result<TValue>(TValue? value) => value is null ? Failure(value) : Success(value);
		public static implicit operator TValue?(Result<TValue> result) => result.Value;

		public static Result<TValue> Success(TValue? value) => new(value, true, Error.None);
		public static Result<TValue> Failure(TValue? value) => new(value, false, Error.Default);
		public static Result<TValue> Failure(TValue? value, Error error) => new(value, false, error);
		public static Result<TValue> Failure(TValue? value, IEnumerable<Error> errors) => new(value, false, errors);

		public new Result<TValue> Ensure(Func<bool> predicate, Error error)
		{
			ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
			ArgumentNullException.ThrowIfNull(error, nameof(error));

			if (predicate.Invoke() == false)
			{
				_status = false;
				_errors.Add(error);
			}

			return this;
		}

		public Result<TValue> Ensure(Func<TValue?, bool> predicate, Error error)
		{
			ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
			ArgumentNullException.ThrowIfNull(error, nameof(error));

			if (predicate.Invoke(_value) == false)
			{
				_status = false;
				_errors.Add(error);
			}

			return this;
		}

		public Result<TValue> Ensure(params (Func<TValue?, bool> predicate, Error error)[] validators)
		{
			if (validators == null || validators.Length == 0)
				throw new ArgumentNullException(nameof(validators));

			var results = new List<Result<TValue>>();

			foreach (var (predicate, error) in validators)
			{
				results.Add(Ensure(predicate, error));
			}

			return Combine([.. results]);
		}

		public Result<TOut> Map<TOut>(Func<TValue?, TOut> mappingFunc)
		{
			ArgumentNullException.ThrowIfNull(mappingFunc, nameof(mappingFunc));

			return IsSuccess ? new(mappingFunc.Invoke(this), true, Error.None) : new(mappingFunc.Invoke(this), false, _errors);
		}

		public static Result<TValue> Ensure(TValue? value, Func<TValue?, bool> predicate, Error error) => Success(value).Ensure(predicate, error);
		public static Result<TValue> Ensure(TValue? value, params (Func<TValue?, bool> predicate, Error error)[] validators) => Success(value).Ensure(validators);
		public static Result<TOut> Map<TOut>(Result<TValue> result, Func<TValue?, TOut> mappingFunc) => result.Map(mappingFunc);

		public static Result<TValue> Combine(params Result<TValue>[] results)
		{
			if (results == null || results.Length == 0)
				throw new ArgumentNullException(nameof(results));

			if (results.Any(i => i.IsFailure))
				return Failure(results[0]._value, results.SelectMany(r => r.Errors).Distinct());

			return Success(results[0]._value);
		}
	}
}
