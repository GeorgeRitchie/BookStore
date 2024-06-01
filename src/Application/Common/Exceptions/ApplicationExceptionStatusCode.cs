using Domain.Shared.ExceptionAbstractions;

namespace Application.Common.Exceptions
{
    public sealed class ApplicationExceptionStatusCode : ExceptionStatusCode
    {
        #region Application Exception status codes - for public only

        #endregion

        #region Application Exception status codes - for internal only
        /// <summary>
        /// Gets an unauthenticated user exceptions status code. (P-A0001)
        /// </summary>
        public static readonly ApplicationExceptionStatusCode UnauthenticatedUserException =
                                new("Application.Exceptions.UserUnauthenticated", "P-A0001");

        /// <summary>
        /// Gets an access forbidden exception status code. (P-A0002)
        /// </summary>
        public static readonly ApplicationExceptionStatusCode AccessForbiddenException =
                                new("Application.Exceptions.AccessForbidden", "P-A0002");

        /// <summary>
        /// Gets a fluent validation exception status code. (P-A0003)
        /// </summary>
        public static readonly ApplicationExceptionStatusCode FluentValidationException =
                                new("Application.Exceptions.FluentValidation", "P-A0003");
        #endregion

        #region Application Exception status codes - for both
        /// <summary>
        /// Gets a universal application exception status code. (A0001)
        /// </summary>
        public static readonly ApplicationExceptionStatusCode UniversalApplicationException =
                                new("Application.Exceptions.UniversalException", "A0001");

        /// <summary>
        /// Gets an operation failed application exception status code. (A0002)
        /// </summary>
        public static readonly ApplicationExceptionStatusCode OperationFailedApplicationException =
                                new("Application.Exceptions.OperationFailed", "A0002");
        #endregion

        private ApplicationExceptionStatusCode(string name, string value) : base(name, value)
        {
        }
    }
}
