using System.Net;

namespace TaskProcessing.Core.Models.ValueObjects
{
    public record TaskResponse(HttpStatusCode StatusCode, string Message);
}
