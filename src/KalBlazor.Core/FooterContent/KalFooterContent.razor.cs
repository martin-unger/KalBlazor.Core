using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalFooterContent
{
    protected override string ComponentClass => "kal-footer-content";

    protected override string DefaultClass => "mt-auto w-full text-center pt-2.5";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool FixedWhenContentIsShort { get; set; }

    protected override string DynamicClass => FixedWhenContentIsShort ? "sticky bottom-0 z-40" : string.Empty;
}
