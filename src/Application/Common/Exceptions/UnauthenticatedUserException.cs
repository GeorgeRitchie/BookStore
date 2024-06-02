using Domain.Shared.ExceptionAbstractions;

namespace Application.Common.Exceptions
{
    public class UnauthenticatedUserException : ApplicationException, IPublicException
    {
        public UnauthenticatedUserException() : this(null)
        {
        }

        public UnauthenticatedUserException(string? message) : this(message, null, null, null)
        {
        }

        public UnauthenticatedUserException(string? message,
                                      string? description,
                                      string? source,
                                      Exception? innerException = null) : base(ApplicationExceptionStatusCode.UnauthenticatedUserException,
                                                    message,
                                                    description,
                                                    source,
                                                    innerException)
        {
        }
    }
}
