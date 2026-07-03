namespace SoftwareThingies.KalBlazor.Core.DataGrid;

public sealed record KalDataGridChildRowHeaderTemplateContext(
    bool AreAllRowsExpanded,
    string AriaLabel,
    Func<Task> ToggleAsync);

public sealed record KalDataGridChildRowToggleTemplateContext<TItem>(
    TItem Item,
    bool IsExpanded,
    string AriaLabel,
    Func<Task> ToggleAsync);
