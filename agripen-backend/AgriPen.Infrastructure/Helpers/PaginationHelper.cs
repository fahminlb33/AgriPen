namespace AgriPen.Infrastructure.Helpers;

public static class PaginationHelper
{
    public static int GetOffset(int page, int limit)
    {
        return (page - 1) * limit;
    }
}
