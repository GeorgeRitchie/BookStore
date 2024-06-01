using Domain.Shared.ExceptionAbstractions;
using FluentValidation.Results;

namespace Application.Common.Exceptions
{
    public class FluentValidationException : ApplicationException, IPublicException
    {
        public IDictionary<string, string[]> Errors { get; }

        public FluentValidationException(IEnumerable<ValidationFailure> failures,
                                   string? description,
                                   string? source,
                                   Exception? innerException = null) : this(failures,
                                                    "One or more validation errors occurred.",
                                                    description,
                                                    source,
                                                    innerException)
        {
        }

        public FluentValidationException(IEnumerable<ValidationFailure> failures,
                                   string? message,
                                   string? description,
                                   string? source,
                                   Exception? innerException = null) : base(ApplicationExceptionStatusCode.FluentValidationException,
                                                    message,
                                                    description,
                                                    source,
                                                    innerException)
        {
            ArgumentNullException.ThrowIfNull(failures);

            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}
