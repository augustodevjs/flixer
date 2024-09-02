using Flixer.Catalog.Application.Common.Output;
using Flixer.Catalog.Application.Common.Output.Common;

namespace Flixer.Catalog.Api.Response;

public class ApiResponseList<TItemData> : ApiResponse<IReadOnlyList<TItemData>>
{
    public ApiResponseListMeta Meta { get; private set; }
    
    public ApiResponseList(
        PaginatedListOutput<TItemData> paginetedListOutput
    ) : base(paginetedListOutput.Items)
    {
        Meta = new ApiResponseListMeta(
            paginetedListOutput.Page, 
            paginetedListOutput.PerPage, 
            paginetedListOutput.Total
        );
    }
    
    public ApiResponseList(
        int currentPage,
        int perPage,
        int total,
        IReadOnlyList<TItemData> data) : base(data)
    {
        Meta = new ApiResponseListMeta(currentPage, perPage, total);
    }
}