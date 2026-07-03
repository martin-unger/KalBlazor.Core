using Microsoft.AspNetCore.Components;
using SoftwareThingies.KalBlazor.Core;

namespace SoftwareThingies.KalBlazor.Core.DataGrid;

public partial class KalDataGrid<TItem> : IDisposable, IKalDataGridFilterContext
{
    private const string DefaultTableClass = "w-full min-w-full border-collapse text-sm";
    private const string DefaultHeaderClass = "";
    private const string DefaultHeaderRowClass = "";
    private const string DefaultBodyClass = "divide-y";
    private const string DefaultRowClass = "transition-colors";
    private const string DefaultExpandHeaderCellClass = "w-0 px-3 py-3";
    private const string DefaultExpandAllButtonClass = "inline-flex h-7 w-7 items-center justify-center rounded-md text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900 focus:outline-none focus:ring-2 focus:ring-amber-200";
    private const string DefaultExpandCellClass = "w-0 px-3 py-3 align-top";
    private const string DefaultExpandButtonClass = "inline-flex h-7 w-7 items-center justify-center rounded-md text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900 focus:outline-none focus:ring-2 focus:ring-amber-200";
    private const string DefaultChildRowClass = "kal-data-grid-child-row";
    private const string DefaultChildCellClass = "bg-slate-50 px-4 py-4";
    private const string DefaultChildContentClass = "w-full";
    protected override string ComponentClass => "kal-data-grid";

    protected override string DefaultClass => "w-full";

    private readonly HashSet<TItem> _expandedItems = [];
    private bool _defaultExpansionApplied;
    private bool? _pendingDescendantExpansion;
    private KalDataGridChildRowExpansionContext? _subscribedParentChildRowExpansionContext;
    private int _lastHandledParentExpansionVersion;

    private KalDataGridContext<TItem> GridContext { get; } = new();

    private KalDataGridChildRowExpansionContext ChildRowExpansionContext { get; } = new();

    [CascadingParameter]
    private KalDataGridChildRowExpansionContext? ParentChildRowExpansionContext { get; set; }

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
    public string ExpandAllButtonCollapsedAriaLabel { get; set; } = "Alle Details anzeigen";

    [Parameter]
    public string ExpandAllButtonExpandedAriaLabel { get; set; } = "Alle Details ausblenden";

    [Parameter]
    public string? ExpandHeaderCellClass { get; set; }

    [Parameter]
    public string? AdditionalExpandHeaderCellClass { get; set; }

    [Parameter]
    public string? ExpandAllButtonClass { get; set; }

    [Parameter]
    public string? AdditionalExpandAllButtonClass { get; set; }

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

    private bool HasDefaultChildRowColumn => HasChildRowContent && GridContext.ChildRowColumn is null;

    private int ChildRowColSpan => GridContext.Entries.Count + (HasDefaultChildRowColumn ? 1 : 0);

    private bool AreAllBoundChildRowsExpanded
    {
        get
        {
            var items = BoundItems.ToArray();
            return items.Length > 0 && items.All(_expandedItems.Contains);
        }
    }

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

    private string ExpandAllButtonCssClass => $"{(string.IsNullOrWhiteSpace(ExpandAllButtonClass) ? DefaultExpandAllButtonClass : ExpandAllButtonClass)} {AdditionalExpandAllButtonClass}".Trim();

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

    private Task ToggleChildRowAsync(TItem item)
    {
        if (!_expandedItems.Add(item))
        {
            _expandedItems.Remove(item);
        }

        return InvokeAsync(StateHasChanged);
    }

    private string GetExpandButtonAriaLabel(bool isChildRowExpanded)
    {
        return isChildRowExpanded
            ? ExpandButtonExpandedAriaLabel
            : ExpandButtonCollapsedAriaLabel;
    }

    private string GetExpandAllButtonAriaLabel()
    {
        return AreAllBoundChildRowsExpanded
            ? ExpandAllButtonExpandedAriaLabel
            : ExpandAllButtonCollapsedAriaLabel;
    }

    private Task ToggleAllChildRowsAsync()
    {
        SetAllChildRowsExpanded(!AreAllBoundChildRowsExpanded);
        return InvokeAsync(StateHasChanged);
    }

    private KalDataGridChildRowHeaderTemplateContext CreateChildRowHeaderTemplateContext()
    {
        return new KalDataGridChildRowHeaderTemplateContext(
            AreAllBoundChildRowsExpanded,
            GetExpandAllButtonAriaLabel(),
            ToggleAllChildRowsAsync);
    }

    private KalDataGridChildRowToggleTemplateContext<TItem> CreateChildRowToggleTemplateContext(TItem item, bool isExpanded)
    {
        return new KalDataGridChildRowToggleTemplateContext<TItem>(
            item,
            isExpanded,
            GetExpandButtonAriaLabel(isExpanded),
            () => ToggleChildRowAsync(item));
    }

    private RenderFragment RenderChildRowHeader(KalToggleGridColumn<TItem> column)
    {
        return column.CellHeaderTemplate is not null
            ? column.CellHeaderTemplate(CreateChildRowHeaderTemplateContext())
            : RenderDefaultChildRowHeader();
    }

    private RenderFragment RenderChildRowToggle(KalToggleGridColumn<TItem> column, TItem item, bool isExpanded)
    {
        return column.CellTemplate is not null
            ? column.CellTemplate(CreateChildRowToggleTemplateContext(item, isExpanded))
            : RenderDefaultChildRowToggle(item, isExpanded);
    }

    private RenderFragment RenderDefaultChildRowHeader()
    {
        return builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "type", "button");
            builder.AddAttribute(2, "class", ExpandAllButtonCssClass);
            builder.AddAttribute(3, "aria-expanded", AreAllBoundChildRowsExpanded);
            builder.AddAttribute(4, "aria-label", GetExpandAllButtonAriaLabel());
            builder.AddAttribute(5, "onclick", EventCallback.Factory.Create(this, ToggleAllChildRowsAsync));
            builder.OpenComponent<KalIcon>(6);
            builder.AddAttribute(7, "Icon", AreAllBoundChildRowsExpanded ? Icons.AngleUp : Icons.AngleDown);
            builder.CloseComponent();
            builder.CloseElement();
        };
    }

    private RenderFragment RenderDefaultChildRowToggle(TItem item, bool isExpanded)
    {
        return builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "type", "button");
            builder.AddAttribute(2, "class", ExpandButtonCssClass);
            builder.AddAttribute(3, "aria-expanded", isExpanded);
            builder.AddAttribute(4, "aria-label", GetExpandButtonAriaLabel(isExpanded));
            builder.AddAttribute(5, "onclick", EventCallback.Factory.Create(this, () => ToggleChildRowAsync(item)));
            builder.OpenComponent<KalIcon>(6);
            builder.AddAttribute(7, "Icon", isExpanded ? Icons.AngleDown : Icons.AngleRight);
            builder.CloseComponent();
            builder.CloseElement();
        };
    }

    private void SetAllChildRowsExpanded(bool isExpanded, bool propagateToDescendants = true)
    {
        foreach (var item in BoundItems.ToArray())
        {
            if (isExpanded)
            {
                _expandedItems.Add(item);
            }
            else
            {
                _expandedItems.Remove(item);
            }
        }

        if (propagateToDescendants)
        {
            _pendingDescendantExpansion = isExpanded;
        }
    }

    private Task OnParentChildRowExpansionRequested(KalDataGridChildRowExpansionRequest request)
    {
        if (request.Version <= _lastHandledParentExpansionVersion)
        {
            return Task.CompletedTask;
        }

        if (Items is null)
        {
            return Task.CompletedTask;
        }

        _lastHandledParentExpansionVersion = request.Version;
        SetAllChildRowsExpanded(request.IsExpanded);

        return InvokeAsync(StateHasChanged);
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
        if (!ReferenceEquals(_subscribedParentChildRowExpansionContext, ParentChildRowExpansionContext))
        {
            if (_subscribedParentChildRowExpansionContext is not null)
            {
                _subscribedParentChildRowExpansionContext.ExpansionRequested -= OnParentChildRowExpansionRequested;
            }

            _subscribedParentChildRowExpansionContext = ParentChildRowExpansionContext;

            if (_subscribedParentChildRowExpansionContext is not null)
            {
                _subscribedParentChildRowExpansionContext.ExpansionRequested += OnParentChildRowExpansionRequested;
            }
        }

        if (ParentChildRowExpansionContext?.CurrentRequest is { } request
            && request.Version > _lastHandledParentExpansionVersion)
        {
            if (Items is not null)
            {
                _lastHandledParentExpansionVersion = request.Version;
                SetAllChildRowsExpanded(request.IsExpanded);
            }
        }

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_pendingDescendantExpansion is not { } isExpanded)
        {
            return;
        }

        _pendingDescendantExpansion = null;
        await ChildRowExpansionContext.RequestExpansionAsync(isExpanded);
    }

    private void OnGridContextStateChanged()
    {
        _ = InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        GridContext.StateChanged -= OnGridContextStateChanged;

        if (_subscribedParentChildRowExpansionContext is not null)
        {
            _subscribedParentChildRowExpansionContext.ExpansionRequested -= OnParentChildRowExpansionRequested;
        }
    }
}
