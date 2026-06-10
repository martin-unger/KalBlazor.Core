namespace SoftwareThingies.KalBlazor.Core;

public sealed class KalDrawerStateContext
{
    internal KalDrawerStateContext(
        KalDrawerState state,
        string? id,
        string? name,
        KalDrawerSide side,
        KalDrawerVariant variant)
    {
        State = state;
        Id = id;
        Name = name;
        Side = side;
        Variant = variant;
    }

    public KalDrawerState State { get; }

    public bool IsHidden => State == KalDrawerState.Hidden;

    public bool IsMinimized => State == KalDrawerState.Minimized;

    public bool IsMaximized => State == KalDrawerState.Maximized;

    public string? Id { get; }

    public string? Name { get; }

    public KalDrawerSide Side { get; }

    public KalDrawerVariant Variant { get; }
}
