using Domain.Shared.ExceptionAbstractions;

namespace Application.Common.Exceptions
{
    public class AccessForbiddenException : ApplicationException, IPublicException
    {
        public AccessForbiddenException() : this(null)
        {
        }

        public AccessForbiddenException(string? message) : this(message, null, null, null)
        {
        }

        public AccessForbiddenException(string? message,
                                  string? description,
                                  string? source,
                                  Exception? innerException = null) : base(ApplicationExceptionStatusCode.AccessForbiddenException,
                                                   message,
                                                   description,
                                                   source,
                                                   innerException)
        {
        }
    }
}
