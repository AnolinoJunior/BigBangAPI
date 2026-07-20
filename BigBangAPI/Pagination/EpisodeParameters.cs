namespace BigBangAPI.Pagination;

public class EpisodeParameters
{
    private const int MaxPageSize = 40;

    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value < 1)
                _pageSize = 10;
            else
                _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}