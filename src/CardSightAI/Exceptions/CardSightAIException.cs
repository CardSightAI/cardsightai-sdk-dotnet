using System;
using System.Net;

namespace CardSightAI.Exceptions
{
    /// <summary>
    /// Base exception for all CardSight AI SDK exceptions
    /// </summary>
    public class CardSightAIException : Exception
    {
        /// <summary>
        /// The HTTP status code associated with the error, if applicable
        /// </summary>
        public HttpStatusCode? StatusCode { get; }

        /// <summary>
        /// The request ID for tracking the failed request
        /// </summary>
        public string? RequestId { get; }

        /// <summary>
        /// The error code returned by the API
        /// </summary>
        public string? ErrorCode { get; }

        /// <summary>
        /// The raw response body from the API
        /// </summary>
        public string? ResponseBody { get; }

        /// <summary>
        /// Initializes a new instance of the CardSightAIException class
        /// </summary>
        public CardSightAIException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CardSightAIException class with inner exception
        /// </summary>
        public CardSightAIException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CardSightAIException class with detailed error information
        /// </summary>
        public CardSightAIException(
            string message,
            HttpStatusCode? statusCode = null,
            string? requestId = null,
            string? errorCode = null,
            string? responseBody = null,
            Exception? innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            RequestId = requestId;
            ErrorCode = errorCode;
            ResponseBody = responseBody;
        }
    }

    /// <summary>
    /// Exception thrown when authentication fails
    /// </summary>
    public class CardSightAIAuthenticationException : CardSightAIException
    {
        /// <summary>
        /// Initializes a new instance of the CardSightAIAuthenticationException class
        /// </summary>
        /// <param name="message">The error message</param>
        public CardSightAIAuthenticationException(string message)
            : base(message, HttpStatusCode.Unauthorized)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CardSightAIAuthenticationException class with detailed information
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="requestId">The request ID for tracking</param>
        /// <param name="responseBody">The raw response body</param>
        public CardSightAIAuthenticationException(string message, string? requestId, string? responseBody)
            : base(message, HttpStatusCode.Unauthorized, requestId, "AUTHENTICATION_FAILED", responseBody)
        {
        }
    }

    /// <summary>
    /// Exception thrown when a requested resource is not found
    /// </summary>
    public class CardSightAINotFoundException : CardSightAIException
    {
        /// <summary>
        /// Initializes a new instance of the CardSightAINotFoundException class
        /// </summary>
        /// <param name="message">The error message</param>
        public CardSightAINotFoundException(string message)
            : base(message, HttpStatusCode.NotFound)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CardSightAINotFoundException class with detailed information
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="requestId">The request ID for tracking</param>
        /// <param name="responseBody">The raw response body</param>
        public CardSightAINotFoundException(string message, string? requestId, string? responseBody)
            : base(message, HttpStatusCode.NotFound, requestId, "NOT_FOUND", responseBody)
        {
        }
    }

    /// <summary>
    /// Exception thrown when validation fails
    /// </summary>
    public class CardSightAIValidationException : CardSightAIException
    {
        /// <summary>
        /// Initializes a new instance of the CardSightAIValidationException class
        /// </summary>
        /// <param name="message">The error message</param>
        public CardSightAIValidationException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CardSightAIValidationException class with detailed information
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="requestId">The request ID for tracking</param>
        /// <param name="responseBody">The raw response body</param>
        public CardSightAIValidationException(string message, string? requestId, string? responseBody)
            : base(message, HttpStatusCode.BadRequest, requestId, "VALIDATION_FAILED", responseBody)
        {
        }
    }

    /// <summary>
    /// Exception thrown when rate limit is exceeded
    /// </summary>
    public class CardSightAIRateLimitException : CardSightAIException
    {
        /// <summary>
        /// The time to wait before retrying
        /// </summary>
        public TimeSpan? RetryAfter { get; }

        /// <summary>
        /// Initializes a new instance of the CardSightAIRateLimitException class
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="retryAfter">The time to wait before retrying</param>
        public CardSightAIRateLimitException(string message, TimeSpan? retryAfter = null)
            : base(message, HttpStatusCode.TooManyRequests)
        {
            RetryAfter = retryAfter;
        }

        /// <summary>
        /// Initializes a new instance of the CardSightAIRateLimitException class with detailed information
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="requestId">The request ID for tracking</param>
        /// <param name="responseBody">The raw response body</param>
        /// <param name="retryAfter">The time to wait before retrying</param>
        public CardSightAIRateLimitException(string message, string? requestId, string? responseBody, TimeSpan? retryAfter = null)
            : base(message, HttpStatusCode.TooManyRequests, requestId, "RATE_LIMIT_EXCEEDED", responseBody)
        {
            RetryAfter = retryAfter;
        }
    }

    /// <summary>
    /// Exception thrown when a request times out
    /// </summary>
    public class CardSightAITimeoutException : CardSightAIException
    {
        /// <summary>
        /// Initializes a new instance of the CardSightAITimeoutException class
        /// </summary>
        /// <param name="message">The error message</param>
        public CardSightAITimeoutException(string message)
            : base(message, HttpStatusCode.RequestTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CardSightAITimeoutException class with inner exception
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="innerException">The inner exception that caused the timeout</param>
        public CardSightAITimeoutException(string message, Exception innerException)
            : base(message, HttpStatusCode.RequestTimeout, null, "TIMEOUT", null, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown for server errors
    /// </summary>
    public class CardSightAIServerException : CardSightAIException
    {
        /// <summary>
        /// Initializes a new instance of the CardSightAIServerException class
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="statusCode">The HTTP status code</param>
        public CardSightAIServerException(string message, HttpStatusCode statusCode)
            : base(message, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CardSightAIServerException class with detailed information
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="statusCode">The HTTP status code</param>
        /// <param name="requestId">The request ID for tracking</param>
        /// <param name="responseBody">The raw response body</param>
        public CardSightAIServerException(string message, HttpStatusCode statusCode, string? requestId, string? responseBody)
            : base(message, statusCode, requestId, "SERVER_ERROR", responseBody)
        {
        }
    }
}