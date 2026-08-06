namespace BigBangAPI.Pagination;

public class CharacterParameters
{
    private const int MaxPageSize = 6;

    public int PageNumber { get; set; } = 1;

    private int _pageSize = 6;

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value < 1)
                _pageSize = 6;
            else
                _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }

    public string? Name { get; set; }
}