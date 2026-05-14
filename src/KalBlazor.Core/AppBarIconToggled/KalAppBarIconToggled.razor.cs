using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalAppBarIconToggled
{
    protected override string DefaultClass => "absolute inset-0 flex items-center justify-end transition-opacity duration-150";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [CascadingParameter]
    internal KalAppBarContext? AppBarContext { get; set; }

    protected override string AdditionalClass
    {
        get
        {
            var visibilityClass = AppBarContext?.IsOpen == true ? "opacity-100" : "opacity-0 pointer-events-none";
            var alignmentClass = AppBarContext?.ToggleHorizontalAlignment == KalHorizontalAlignment.Right ? "justify-end" : "justify-start";

            return $"{visibilityClass} {alignmentClass}";
        }
    }
}
