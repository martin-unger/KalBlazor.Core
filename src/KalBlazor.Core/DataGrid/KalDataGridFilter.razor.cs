using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDataGridFilter : BaseComponent
{
    private const string DefaultInputClass = "w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-950 outline-none transition-colors placeholder:text-slate-400 focus:border-amber-500 focus:ring-2 focus:ring-amber-200";
    private const string DefaultResetButtonClass = "rounded-md border border-slate-300 bg-white px-3 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-50";

    protected override string ComponentClass => "kal-data-grid-filter";

    protected override string DefaultClass => "flex items-center gap-2 border-b border-slate-200 bg-white p-3";

    [Parameter]
    public string Placeholder { get; set; } = "Filter...";

    [Parameter]
    public string? InputClass { get; set; }

    [Parameter]
    public string? AdditionalInputClass { get; set; }

    [Parameter]
    public bool ShowResetButton { get; set; } = true;

    [Parameter]
    public string ResetButtonText { get; set; } = "Reset";

    [Parameter]
    public string? ResetButtonClass { get; set; }

    [Parameter]
    public string? AdditionalResetButtonClass { get; set; }

    [CascadingParameter]
    internal IKalDataGridFilterContext? FilterContext { get; set; }

    private string InputCssClass => $"{(string.IsNullOrWhiteSpace(InputClass) ? DefaultInputClass : InputClass)} {AdditionalInputClass}".Trim();

    private string ResetButtonCssClass => $"{(string.IsNullOrWhiteSpace(ResetButtonClass) ? DefaultResetButtonClass : ResetButtonClass)} {AdditionalResetButtonClass}".Trim();

    private bool HasFilterText => !string.IsNullOrWhiteSpace(FilterContext?.FilterText);

    protected override void OnParametersSet()
    {
        if (FilterContext is null)
        {
            throw new InvalidOperationException($"{nameof(KalDataGridFilter)} must be placed inside the {nameof(KalDataGrid<object>)} Filter template.");
        }
    }

    private Task OnInput(ChangeEventArgs args)
    {
        return FilterContext!.SetFilterTextAsync(args.Value?.ToString());
    }

    private Task ResetAsync()
    {
        return FilterContext!.ClearFilterTextAsync();
    }
}
