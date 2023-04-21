namespace CodingAssessment.Api.Helpers
{
    internal class BadRequestException : Exception
    {
        public readonly ErrorResponse ErrorResponse;
        public BadRequestException(string message) : base(message)
        {
            ErrorResponse = new ErrorResponse
            {
                Message = message,
                StackTrace = nameof(BadRequestException)
            };
        }
    }

    internal class NotFoundException : Exception
    {
        public readonly ErrorResponse ErrorResponse;
        public NotFoundException(string message) : base(message)
        {
            ErrorResponse = new ErrorResponse
            {
                Message = message,
                StackTrace = nameof(NotFoundException)
            };
        }
    }

    internal class ConflictException : Exception
    {
        public readonly ErrorResponse ErrorResponse;
        public ConflictException(string message) : base(message)
        {
            ErrorResponse = new ErrorResponse
            {
                Message = message,
                StackTrace = nameof(ConflictException)
            };
        }
    }
}
