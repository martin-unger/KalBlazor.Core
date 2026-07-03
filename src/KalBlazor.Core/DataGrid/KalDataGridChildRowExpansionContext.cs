namespace SoftwareThingies.KalBlazor.Core.DataGrid;

public sealed class KalDataGridChildRowExpansionContext
{
    public event Func<KalDataGridChildRowExpansionRequest, Task>? ExpansionRequested;

    public KalDataGridChildRowExpansionRequest? CurrentRequest { get; private set; }

    internal async Task RequestExpansionAsync(bool isExpanded)
    {
        CurrentRequest = new KalDataGridChildRowExpansionRequest(isExpanded, (CurrentRequest?.Version ?? 0) + 1);

        if (ExpansionRequested is null)
        {
            return;
        }

        var handlers = ExpansionRequested
            .GetInvocationList()
            .Cast<Func<KalDataGridChildRowExpansionRequest, Task>>();

        foreach (var handler in handlers)
        {
            await handler(CurrentRequest);
        }
    }
}

public sealed record KalDataGridChildRowExpansionRequest(
    bool IsExpanded,
    int Version);
