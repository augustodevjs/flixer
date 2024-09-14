using Flixer.Catalog.Application.Common.Output.Common;

namespace Flixer.Catalog.Application.Common.Output.Video;

public class ListVideosOutput : PaginatedListOutput<VideoOutput>
{
    public ListVideosOutput(
        int page, 
        int perPage, 
        int total, 
        IReadOnlyList<VideoOutput> items) 
        : base(page, total, perPage, items)
    { }
}
