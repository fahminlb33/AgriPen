using AgriPen.Infrastructure.Converters;
using AgriPen.SharedKernel.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace AgriPen.Infrastructure.Extensions;

public static class EFExtensions
{
    public static IOrderedQueryable<K> ApplySort<K, V>(this IQueryable<K> query, Expression<Func<K, V>> keySelector, SortDirection direction)
    {
        return direction == SortDirection.Ascending 
            ? query.OrderBy(keySelector) 
            : query.OrderByDescending(keySelector);
    }
}
