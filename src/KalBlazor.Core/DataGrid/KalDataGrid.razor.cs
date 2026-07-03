using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core.DataGrid;

public partial class KalDataGrid<TItem> : IDisposable, IKalDataGridFilterContext
{
    private const string DefaultTableClass = "w-full min-w-full border-collapse text-sm";
    private const string DefaultHeaderClass = "";
    private const string DefaultHeaderRowClass = "";
    private const string DefaultBodyClass = "divide-y";
    private const string DefaultRowClass = "transition-colors";
    private const string DefaultExpandHeaderCellClass = "w-0 px-3 py-3";
    private const string DefaultExpandCellClass = "w-0 px-3 py-3 align-top";
    private const string DefaultExpandButtonClass = "inline-flex h-7 w-7 items-center justify-center rounded-md text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900 focus:outline-none focus:ring-2 focus:ring-amber-200";
    private const string DefaultChildRowClass = "kal-data-grid-child-row";
    private const string DefaultChildCellClass = "bg-slate-50 px-4 py-4";
    private const string DefaultChildContentClass = "w-full";
    protected override string ComponentClass => "kal-data-grid";

    protected override string DefaultClass => "w-full";

    private readonly HashSet<TItem> _expandedItems = [];
    private bool _defaultExpansionApplied;

    private KalDataGridContext<TItem> GridContext { get; } = new();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }

    [Parameter]
    public RenderFragment? Columns { get; set; }

    [Parameter]
    public RenderFragment<TItem>? ChildRowContent { get; set; }

    [Parameter]
    public RenderFragment? Filter { get; set; }

    [Parameter]
    public bool ShowFilter { get; set; }

    [Parameter]
    public string FilterPlaceholder { get; set; } = "Filter...";

    [Parameter]
    public string? FilterText { get; set; }

    [Parameter]
    public EventCallback<string?> FilterTextChanged { get; set; }

    [Parameter]
    public string? FilterClass { get; set; }

    [Parameter]
    public string? AdditionalFilterClass { get; set; }

    [Parameter]
    public string? FilterInputClass { get; set; }

    [Parameter]
    public string? AdditionalFilterInputClass { get; set; }

    /// <summary>
    /// Replaces the default table classes.
    /// </summary>
    [Parameter]
    public string? TableClass { get; set; }

    /// <summary>
    /// Additional classes appended to the table classes.
    /// </summary>
    [Parameter]
    public string? AdditionalTableClass { get; set; }

    [Parameter]
    public string? HeaderClass { get; set; }

    [Parameter]
    public string? AdditionalHeaderClass { get; set; }

    [Parameter]
    public string? HeaderRowClass { get; set; }

    [Parameter]
    public string? AdditionalHeaderRowClass { get; set; }

    [Parameter]
    public string? AdditionalHeaderCellClass { get; set; }

    [Parameter]
    public string? BodyClass { get; set; }

    [Parameter]
    public string? AdditionalBodyClass { get; set; }

    [Parameter]
    public string? RowClass { get; set; }

    [Parameter]
    public string? AdditionalRowClass { get; set; }

    [Parameter]
    public bool ExpandChildRowsByDefault { get; set; }

    [Parameter]
    public string ExpandHeaderText { get; set; } = "Details";

    [Parameter]
    public string ExpandButtonCollapsedAriaLabel { get; set; } = "Details anzeigen";

    [Parameter]
    public string ExpandButtonExpandedAriaLabel { get; set; } = "Details ausblenden";

    [Parameter]
    public string? ExpandHeaderCellClass { get; set; }

    [Parameter]
    public string? AdditionalExpandHeaderCellClass { get; set; }

    [Parameter]
    public string? ExpandCellClass { get; set; }

    [Parameter]
    public string? AdditionalExpandCellClass { get; set; }

    [Parameter]
    public string? ExpandButtonClass { get; set; }

    [Parameter]
    public string? AdditionalExpandButtonClass { get; set; }

    [Parameter]
    public string? ChildRowClass { get; set; }

    [Parameter]
    public string? AdditionalChildRowClass { get; set; }

    [Parameter]
    public string? ChildCellClass { get; set; }

    [Parameter]
    public string? AdditionalChildCellClass { get; set; }

    [Parameter]
    public string? ChildContentClass { get; set; }

    [Parameter]
    public string? AdditionalChildContentClass { get; set; }

    private bool IsBound => Items is not null && Columns is not null;

    private bool HasChildRowContent => ChildRowContent is not null;

    private int ChildRowColSpan => GridContext.Columns.Count + (HasChildRowContent ? 1 : 0);

    private IEnumerable<TItem> BoundItems => string.IsNullOrWhiteSpace(FilterText)
        ? Items ?? []
        : (Items ?? []).Where(ItemMatchesFilter);

    private string TableCssClass => $"{(string.IsNullOrWhiteSpace(TableClass) ? DefaultTableClass : TableClass)} {AdditionalTableClass}".Trim();

    private string HeaderCssClass => $"{(string.IsNullOrWhiteSpace(HeaderClass) ? DefaultHeaderClass : HeaderClass)} {AdditionalHeaderClass}".Trim();

    private string HeaderRowCssClass => $"{(string.IsNullOrWhiteSpace(HeaderRowClass) ? DefaultHeaderRowClass : HeaderRowClass)} {AdditionalHeaderRowClass}".Trim();

    private string GetHeaderCellCssClass(KalGridColumn<TItem> column)
    {
        return $"{AdditionalHeaderCellClass} {column.HeaderCssClass}".Trim();
    }

    private string BodyCssClass => $"{(string.IsNullOrWhiteSpace(BodyClass) ? DefaultBodyClass : BodyClass)} {AdditionalBodyClass}".Trim();

    private string RowCssClass => $"{(string.IsNullOrWhiteSpace(RowClass) ? DefaultRowClass : RowClass)} {AdditionalRowClass}".Trim();

    private string ExpandHeaderCellCssClass => $"{(string.IsNullOrWhiteSpace(ExpandHeaderCellClass) ? DefaultExpandHeaderCellClass : ExpandHeaderCellClass)} {AdditionalExpandHeaderCellClass}".Trim();

    private string ExpandCellCssClass => $"{(string.IsNullOrWhiteSpace(ExpandCellClass) ? DefaultExpandCellClass : ExpandCellClass)} {AdditionalExpandCellClass}".Trim();

    private string ExpandButtonCssClass => $"{(string.IsNullOrWhiteSpace(ExpandButtonClass) ? DefaultExpandButtonClass : ExpandButtonClass)} {AdditionalExpandButtonClass}".Trim();

    private string ChildRowCssClass => $"{(string.IsNullOrWhiteSpace(ChildRowClass) ? DefaultChildRowClass : ChildRowClass)} {AdditionalChildRowClass}".Trim();

    private string ChildCellCssClass => $"{(string.IsNullOrWhiteSpace(ChildCellClass) ? DefaultChildCellClass : ChildCellClass)} {AdditionalChildCellClass}".Trim();

    private string ChildContentCssClass => $"{(string.IsNullOrWhiteSpace(ChildContentClass) ? DefaultChildContentClass : ChildContentClass)} {AdditionalChildContentClass}".Trim();

    private bool ItemMatchesFilter(TItem item)
    {
        return GridContext.Columns.Any(column =>
            column.GetSearchText(item).Contains(FilterText!, StringComparison.CurrentCultureIgnoreCase));
    }

    private bool IsChildRowExpanded(TItem item)
    {
        return _expandedItems.Contains(item);
    }

    private void ToggleChildRow(TItem item)
    {
        if (!_expandedItems.Add(item))
        {
            _expandedItems.Remove(item);
        }
    }

    private string GetExpandButtonAriaLabel(bool isChildRowExpanded)
    {
        return isChildRowExpanded
            ? ExpandButtonExpandedAriaLabel
            : ExpandButtonCollapsedAriaLabel;
    }

    async Task IKalDataGridFilterContext.SetFilterTextAsync(string? filterText)
    {
        FilterText = filterText;

        if (FilterTextChanged.HasDelegate)
        {
            await FilterTextChanged.InvokeAsync(FilterText);
        }

        await InvokeAsync(StateHasChanged);
    }

    Task IKalDataGridFilterContext.ClearFilterTextAsync()
    {
        return ((IKalDataGridFilterContext)this).SetFilterTextAsync(null);
    }

    protected override void OnInitialized()
    {
        GridContext.StateChanged += OnGridContextStateChanged;
    }

    protected override void OnParametersSet()
    {
        if (!ExpandChildRowsByDefault || _defaultExpansionApplied || Items is null)
        {
            return;
        }

        foreach (var item in Items)
        {
            _expandedItems.Add(item);
        }

        _defaultExpansionApplied = true;
    }

    private void OnGridContextStateChanged()
    {
        _ = InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        GridContext.StateChanged -= OnGridContextStateChanged;
    }
}
