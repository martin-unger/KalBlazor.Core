using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalCollapsible
{
    protected override string ComponentClass => "kal-collapsible";

    protected override string DefaultClass => "w-full basis-full flex-col gap-4 py-3 md:basis-auto md:self-center md:flex md:w-auto md:flex-row md:items-center md:gap-4 md:py-0";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public KalHorizontalAlignment HorizontalAlignment { get; set; } = KalHorizontalAlignment.Left;

    [CascadingParameter]
    internal KalAppBarContext? AppBarContext { get; set; }

    protected override string DynamicClass
    {
        get
        {
            var visibilityClass = AppBarContext?.IsOpen == true ? "flex" : "hidden";
            var orderClass = AppBarContext?.Bottom == true ? "order-0" : "order-30";
            var alignmentClass = HorizontalAlignment switch
            {
                KalHorizontalAlignment.Center => "items-center md:justify-center",
                KalHorizontalAlignment.Right => "items-end md:items-center md:justify-end",
                _ => "items-start md:items-center md:justify-start"
            };

            return $"{orderClass} {visibilityClass} {alignmentClass}";
        }
    }
}
