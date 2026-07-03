namespace SoftwareThingies.KalBlazor.Core.DataGrid;

internal sealed class KalDataGridContext<TItem>
{
    private readonly List<object> _columns = [];

    public event Action? StateChanged;

    public IReadOnlyList<object> Entries => _columns;

    public IReadOnlyList<KalGridColumn<TItem>> Columns => _columns.OfType<KalGridColumn<TItem>>().ToArray();

    public KalToggleGridColumn<TItem>? ChildRowColumn => _columns.OfType<KalToggleGridColumn<TItem>>().FirstOrDefault();

    public void Register(KalGridColumn<TItem> column)
    {
        if (!_columns.Contains(column))
        {
            _columns.Add(column);
            StateChanged?.Invoke();
        }
    }

    public void Register(KalToggleGridColumn<TItem> column)
    {
        if (!_columns.Contains(column))
        {
            _columns.Add(column);
            StateChanged?.Invoke();
        }
    }

    public void Unregister(KalGridColumn<TItem> column)
    {
        if (_columns.Remove(column))
        {
            StateChanged?.Invoke();
        }
    }

    public void Unregister(KalToggleGridColumn<TItem> column)
    {
        if (_columns.Remove(column))
        {
            StateChanged?.Invoke();
        }
    }
}
