using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core.DataGrid;

public partial class KalGridHeader
{
    private const string DefaultHeaderRowClass = "border-b border-slate-200";

    protected override string ComponentClass => "kal-grid-header";

    protected override string DefaultClass => "bg-slate-50";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Replaces the default header row classes.
    /// </summary>
    [Parameter]
    public string? RowClass { get; set; }

    /// <summary>
    /// Additional classes appended to the header row classes.
    /// </summary>
    [Parameter]
    public string? AdditionalRowClass { get; set; }

    private string HeaderRowCssClass => $"{(string.IsNullOrWhiteSpace(RowClass) ? DefaultHeaderRowClass : RowClass)} {AdditionalRowClass}".Trim();
}
