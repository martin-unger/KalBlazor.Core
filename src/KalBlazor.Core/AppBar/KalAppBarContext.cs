using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

internal sealed class KalAppBarContext
{
    public bool IsOpen { get; set; }

    public bool Bottom { get; set; }

    public KalHorizontalAlignment ToggleHorizontalAlignment { get; set; } = KalHorizontalAlignment.Left;

    public EventCallback StateChanged { get; set; }
}
