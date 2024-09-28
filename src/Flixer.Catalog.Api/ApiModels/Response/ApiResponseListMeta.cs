namespace Flixer.Catalog.Api.ApiModels.Response;

public class ApiResponseListMeta
{
    public int CurrentPage { get; set; }
    public int Page { get; set; }
    public int Total { get; set; }

    public ApiResponseListMeta(int currentPage, int page, int total)
    {
        Page = page;
        Total = total;
        CurrentPage = currentPage;
    }
}