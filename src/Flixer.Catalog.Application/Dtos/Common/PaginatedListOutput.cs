﻿namespace Flixer.Catalog.Application.Dtos.Common;

public class PaginatedListOutput<TOutputItem>
{
    public int Page { get; set; }
    public int Total { get; set; }
    public int PerPage { get; set; }
    public IReadOnlyList<TOutputItem> Items { get; set; }

    protected PaginatedListOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<TOutputItem> items)
    {
        Page = page;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}
