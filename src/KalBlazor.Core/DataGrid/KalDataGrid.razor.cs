using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core.DataGrid;

public partial class KalDataGrid<TItem> : IDisposable, IKalDataGridFilterContext
{
    private const string DefaultTableClass = "w-full min-w-full border-collapse text-sm";
    private const string DefaultHeaderClass = "bg-slate-50";
    private const string DefaultHeaderRowClass = "border-b border-slate-200";
    private const string DefaultBodyClass = "divide-y divide-slate-200 bg-white";
    private const string DefaultRowClass = "transition-colors hover:bg-slate-50";
    protected override string ComponentClass => "kal-data-grid";

    protected override string DefaultClass => "w-full rounded-lg border border-slate-200 bg-white shadow-sm";

    private KalDataGridContext<TItem> GridContext { get; } = new();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }

    [Parameter]
    public RenderFragment? Columns { get; set; }

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

    private bool IsBound => Items is not null && Columns is not null;

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

    private bool ItemMatchesFilter(TItem item)
    {
        return GridContext.Columns.Any(column =>
            column.GetSearchText(item).Contains(FilterText!, StringComparison.CurrentCultureIgnoreCase));
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

    private void OnGridContextStateChanged()
    {
        _ = InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        GridContext.StateChanged -= OnGridContextStateChanged;
    }
}
