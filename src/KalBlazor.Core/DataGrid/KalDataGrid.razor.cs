using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDataGrid<TItem> : IDisposable
{
    private const string DefaultTableClass = "w-full min-w-full border-collapse text-sm";
    private const string DefaultHeaderClass = "bg-slate-50";
    private const string DefaultHeaderRowClass = "border-b border-slate-200";
    private const string DefaultBodyClass = "divide-y divide-slate-200 bg-white";
    private const string DefaultRowClass = "transition-colors hover:bg-slate-50";
    private const string DefaultFilterClass = "border-b border-slate-200 bg-white p-3";
    private const string DefaultFilterInputClass = "w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-950 outline-none transition-colors placeholder:text-slate-400 focus:border-amber-500 focus:ring-2 focus:ring-amber-200";

    protected override string ComponentClass => "kal-data-grid";

    protected override string DefaultClass => "w-full overflow-x-auto rounded-lg border border-slate-200 bg-white shadow-sm";

    private KalDataGridContext<TItem> GridContext { get; } = new();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }

    [Parameter]
    public RenderFragment? Columns { get; set; }

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

    private string FilterCssClass => $"{(string.IsNullOrWhiteSpace(FilterClass) ? DefaultFilterClass : FilterClass)} {AdditionalFilterClass}".Trim();

    private string FilterInputCssClass => $"{(string.IsNullOrWhiteSpace(FilterInputClass) ? DefaultFilterInputClass : FilterInputClass)} {AdditionalFilterInputClass}".Trim();

    private string HeaderCssClass => $"{(string.IsNullOrWhiteSpace(HeaderClass) ? DefaultHeaderClass : HeaderClass)} {AdditionalHeaderClass}".Trim();

    private string HeaderRowCssClass => $"{(string.IsNullOrWhiteSpace(HeaderRowClass) ? DefaultHeaderRowClass : HeaderRowClass)} {AdditionalHeaderRowClass}".Trim();

    private string BodyCssClass => $"{(string.IsNullOrWhiteSpace(BodyClass) ? DefaultBodyClass : BodyClass)} {AdditionalBodyClass}".Trim();

    private string RowCssClass => $"{(string.IsNullOrWhiteSpace(RowClass) ? DefaultRowClass : RowClass)} {AdditionalRowClass}".Trim();

    private bool ItemMatchesFilter(TItem item)
    {
        return GridContext.Columns.Any(column =>
            column.GetSearchText(item).Contains(FilterText!, StringComparison.CurrentCultureIgnoreCase));
    }

    private async Task OnFilterInput(ChangeEventArgs args)
    {
        FilterText = args.Value?.ToString();

        if (FilterTextChanged.HasDelegate)
        {
            await FilterTextChanged.InvokeAsync(FilterText);
        }
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
