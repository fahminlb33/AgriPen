using AgriPen.SharedKernel.Enums;
using FastEndpoints;
using FluentValidation;
using System.ComponentModel;

namespace AgriPen.SharedKernel.Models;

public class DefaultPaginationRequest
{
    [BindFrom("q"), QueryParam]
    public string? Keyword { get; set; } = string.Empty;

    [BindFrom("sort_by"), QueryParam, DefaultValue("id")]
    public string OrderBy { get; set; } = string.Empty;

    [BindFrom("sort_dir"), QueryParam, DefaultValue(SortDirection.Default)]
    public SortDirection OrderDirection { get; set; }

    [BindFrom("page"), QueryParam, DefaultValue(1)]
    public int Page { get; set; } = 1;

    [BindFrom("limit"), QueryParam, DefaultValue(10)]
    public int Limit { get; set; } = 10;
}

public class DefaultPaginationRequestValidator : Validator<DefaultPaginationRequest>
{
    public DefaultPaginationRequestValidator()
    {
        RuleFor(x => x.OrderDirection).IsInEnum();
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).GreaterThan(0).LessThanOrEqualTo(100);
    }
}

public record PaginationMeta(int Total, int Count, int Page, int Limit)
{
    public int TotalPage => (int)Math.Ceiling(Total/ (double)Limit);
}


public sealed class PaginationResponse<T>
{
    public PaginationResponse(List<T> rows, int total, int page, int limit)
    {
        Meta = new PaginationMeta(total, rows.Count, page, limit);
        Data = rows;
    }

    public PaginationMeta Meta { get; set; }
    public List<T> Data { get; set; }
}