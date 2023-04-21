namespace CodingAssessment.Api.Helpers
{
    internal class ErrorResponse
    {
        public string Message { get; set; }
        public string? StackTrace { get; set; }
        public string TraceId = Guid.NewGuid().ToString();
    }
}