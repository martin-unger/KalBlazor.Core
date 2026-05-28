using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Reflection;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalGridColumn<TItem> : IDisposable
{
    private const string DefaultCollapsedContentClass = "mt-2 space-y-1 text-xs leading-5 text-slate-500";
    private static readonly RenderFragment EmptyCell = builder => { };

    protected override string ComponentClass => "kal-grid-column";

    protected override string DefaultClass => "px-4 py-3 text-xs font-semibold uppercase tracking-wide text-slate-600";

    protected override string DynamicClass => $"{KalDataGridResponsiveClass.TextAlignment(Alignment)} {KalDataGridResponsiveClass.HideBelow(CollapseBelow)} {WidthClass}".Trim();

    [CascadingParameter]
    internal KalDataGridContext<TItem>? GridContext { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public Expression<Func<TItem, object?>>? Property { get; set; }

    [Parameter]
    public RenderFragment<TItem>? CellTemplate { get; set; }

    [Parameter]
    public KalDataGridAlignment Alignment { get; set; } = KalDataGridAlignment.Start;

    /// <summary>
    /// Hides this column below the selected Tailwind breakpoint.
    /// </summary>
    [Parameter]
    public KalDataGridBreakpoint CollapseBelow { get; set; } = KalDataGridBreakpoint.None;

    [Parameter]
    public string Scope { get; set; } = "col";

    [Parameter]
    public string? WidthClass { get; set; }

    [Parameter]
    public string? CellClass { get; set; }

    [Parameter]
    public string? AdditionalCellClass { get; set; }

    [Parameter]
    public RenderFragment<TItem>? CollapsedContent { get; set; }

    [Parameter]
    public KalDataGridBreakpoint CollapsedContentBelow { get; set; } = KalDataGridBreakpoint.Md;

    [Parameter]
    public string? CollapsedContentClass { get; set; }

    [Parameter]
    public string? AdditionalCollapsedContentClass { get; set; }

    internal RenderFragment HeaderContent => ChildContent ?? (builder => builder.AddContent(0, HeaderText));

    internal string HeaderCssClass => CssClass;

    internal string CellCssClass
    {
        get
        {
            var effectiveClass = !string.IsNullOrWhiteSpace(CellClass)
                ? CellClass
                : $"kal-grid-cell px-4 py-3 align-top text-sm text-slate-700 {KalDataGridResponsiveClass.TextAlignment(Alignment)} {KalDataGridResponsiveClass.HideBelow(CollapseBelow)}".Trim();

            return $"{effectiveClass} {AdditionalCellClass}".Trim();
        }
    }

    internal string CollapsedContentCssClass
    {
        get
        {
            var effectiveClass = string.IsNullOrWhiteSpace(CollapsedContentClass)
                ? DefaultCollapsedContentClass
                : CollapsedContentClass;

            return $"{effectiveClass} {KalDataGridResponsiveClass.ShowBelow(CollapsedContentBelow)} {AdditionalCollapsedContentClass}".Trim();
        }
    }

    private string HeaderText => !string.IsNullOrWhiteSpace(Title) ? Title : PropertyName;

    private string PropertyName => TryGetPropertyInfo(Property, out var property) ? property.Name : string.Empty;

    internal RenderFragment RenderCell(TItem item)
    {
        if (CellTemplate is not null)
        {
            return CellTemplate(item);
        }

        if (Property is not null)
        {
            var accessor = Property.Compile();
            var value = accessor(item);
            return builder => builder.AddContent(0, value);
        }

        return EmptyCell;
    }

    protected override void OnInitialized()
    {
        GridContext?.Register(this);
    }

    public void Dispose()
    {
        GridContext?.Unregister(this);
    }

    private static bool TryGetPropertyInfo(Expression<Func<TItem, object?>>? expression, out PropertyInfo property)
    {
        property = null!;

        if (expression?.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo directProperty)
        {
            property = directProperty;
            return true;
        }

        if (expression?.Body is UnaryExpression { Operand: MemberExpression unaryMemberExpression }
            && unaryMemberExpression.Member is PropertyInfo unaryProperty)
        {
            property = unaryProperty;
            return true;
        }

        return false;
    }
}
