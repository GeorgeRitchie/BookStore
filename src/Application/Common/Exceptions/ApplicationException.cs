using Domain.Shared.ExceptionAbstractions;

namespace Application.Common.Exceptions
{
    public abstract class ApplicationException : Exception, IException
    {
        public ExceptionStatusCode StatusCode { get; }
        public string? Description { get; } = null;

        protected ApplicationException() : this(null)
        {
        }

        protected ApplicationException(string? message) : this(ApplicationExceptionStatusCode.UniversalApplicationException,
                                                         message,
                                                         null,
                                                         null,
                                                         null)
        {
        }

        protected ApplicationException(ApplicationExceptionStatusCode statusCode,
                                 string? message,
                                 string? description,
                                 string? source,
                                 Exception? innerException = null) : base(message, innerException)
        {
            StatusCode = statusCode;
            Description = description;
            Source = source;
        }
    }
}
