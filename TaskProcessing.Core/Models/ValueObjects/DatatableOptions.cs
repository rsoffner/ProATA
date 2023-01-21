namespace TaskProcessing.Core.Models.ValueObjects
{
    public record DatatableOptions(DatatablePaginate Paginate, IList<DatatableFilter> Filters);
}
