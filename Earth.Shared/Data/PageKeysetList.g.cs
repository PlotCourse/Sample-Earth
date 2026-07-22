namespace Earth.Shared.Data;

public class PageKeysetList<T>
    where T : class
{
    public int TotalPages { get; set; }
    public int ItemsPerPage { get; set; }
    public int TotalItems { get; set; }
    public List<T> PageKeysets { get; set; }

    public PageKeysetList(
        int totalPages,
        int itemsPerPage,
        int totalItems,
        List<T> pageKeysets)
    {
        TotalPages = totalPages;
        ItemsPerPage = itemsPerPage;
        TotalItems = totalItems;
        PageKeysets = pageKeysets;
    }
}

