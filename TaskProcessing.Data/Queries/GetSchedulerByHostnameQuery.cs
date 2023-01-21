using MediatR;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Data.Queries
{
    public record GetSchedulerByHostnameQuery(string Hostname) : IRequest<Scheduler>;
}
