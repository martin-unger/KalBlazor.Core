using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core.DataGrid;

public partial class KalGridCell
{
    private const string DefaultCollapsedContentClass = "mt-2 space-y-1 text-xs leading-5 text-slate-500";

    protected override string ComponentClass => "kal-grid-cell";

    protected override string DefaultClass => "px-4 py-3 align-top text-sm text-slate-700";

    protected override string DynamicClass => $"{KalDataGridResponsiveClass.TextAlignment(Alignment)} {KalDataGridResponsiveClass.HideBelow(CollapseBelow)}".Trim();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public KalDataGridAlignment Alignment { get; set; } = KalDataGridAlignment.Start;

    /// <summary>
    /// Hides this cell below the selected Tailwind breakpoint.
    /// </summary>
    [Parameter]
    public KalDataGridBreakpoint CollapseBelow { get; set; } = KalDataGridBreakpoint.None;

    /// <summary>
    /// Additional row data rendered inside this cell for compact layouts.
    /// </summary>
    [Parameter]
    public RenderFragment? CollapsedContent { get; set; }

    /// <summary>
    /// Controls the breakpoint where collapsed content becomes hidden again.
    /// </summary>
    [Parameter]
    public KalDataGridBreakpoint CollapsedContentBelow { get; set; } = KalDataGridBreakpoint.Md;

    /// <summary>
    /// Replaces the default collapsed content classes.
    /// </summary>
    [Parameter]
    public string? CollapsedContentClass { get; set; }

    /// <summary>
    /// Additional classes appended to the collapsed content classes.
    /// </summary>
    [Parameter]
    public string? AdditionalCollapsedContentClass { get; set; }

    private string CollapsedContentCssClass
    {
        get
        {
            var effectiveClass = string.IsNullOrWhiteSpace(CollapsedContentClass)
                ? DefaultCollapsedContentClass
                : CollapsedContentClass;

            return $"{effectiveClass} {KalDataGridResponsiveClass.ShowBelow(CollapsedContentBelow)} {AdditionalCollapsedContentClass}".Trim();
        }
    }
}
