namespace SoftwareThingies.KalBlazor.Core;

internal sealed class KalDrawerContext
{
    private readonly Dictionary<string, bool> _openStates = new(StringComparer.Ordinal);
    private readonly Dictionary<string, KalDrawerRegistration> _registrations = new(StringComparer.Ordinal);
    private readonly Dictionary<Guid, KalAppBarRegistration> _appBars = [];

    public event Action? StateChanged;

    public bool AppBarBottom => _appBars.Values.Any(appBar => appBar.Bottom);

    public bool HasAppBar => _appBars.Count > 0;

    public bool HasFixedTopAppBar => _appBars.Values.Any(appBar => appBar.Fixed && !appBar.Bottom);

    public bool HasFixedBottomAppBar => _appBars.Values.Any(appBar => appBar.Fixed && appBar.Bottom);

    public bool HasOpenDrawers => _openStates.Values.Any(isOpen => isOpen);

    public bool HasOpenOverlayDrawers =>
        _openStates.Any(state =>
            state.Value
            && _registrations.TryGetValue(state.Key, out var registration)
            && registration.Variant == KalDrawerVariant.Overlay);

    public DrawerWidth? LeftDockedWidth => GetOpenWidth(KalDrawerSide.Left, KalDrawerVariant.Docked);

    public DrawerWidth? RightDockedWidth => GetOpenWidth(KalDrawerSide.Right, KalDrawerVariant.Docked);

    public DrawerWidth? LeftContentOffsetWidth => GetOpenWidth(KalDrawerSide.Left, KalDrawerVariant.Docked, KalDrawerVariant.Clipped);

    public DrawerWidth? RightContentOffsetWidth => GetOpenWidth(KalDrawerSide.Right, KalDrawerVariant.Docked, KalDrawerVariant.Clipped);

    public bool IsOpen(string key)
    {
        return _openStates.TryGetValue(key, out var isOpen) && isOpen;
    }

    public void Open(string key)
    {
        SetOpen(key, true);
    }

    public void Close(string key)
    {
        SetOpen(key, false);
    }

    public void Toggle(string key)
    {
        SetOpen(key, !IsOpen(key));
    }

    public void CloseAll()
    {
        if (!HasOpenDrawers)
        {
            return;
        }

        foreach (var key in _openStates.Keys.ToArray())
        {
            _openStates[key] = false;
        }

        StateChanged?.Invoke();
    }

    public void Register(string key, KalDrawerSide side, DrawerWidth width, KalDrawerVariant variant)
    {
        var registration = new KalDrawerRegistration(side, width, variant);

        if (_registrations.TryGetValue(key, out var existingRegistration) && existingRegistration == registration)
        {
            return;
        }

        _registrations[key] = registration;
        StateChanged?.Invoke();
    }

    public void RegisterAppBar(Guid key, bool bottom, bool isFixed)
    {
        var registration = new KalAppBarRegistration(bottom, isFixed);

        if (_appBars.TryGetValue(key, out var existingRegistration) && existingRegistration == registration)
        {
            return;
        }

        _appBars[key] = registration;
        StateChanged?.Invoke();
    }

    public void UnregisterAppBar(Guid key)
    {
        if (!_appBars.Remove(key))
        {
            return;
        }

        StateChanged?.Invoke();
    }

    public void Unregister(string key)
    {
        var registrationRemoved = _registrations.Remove(key);
        var stateRemoved = _openStates.Remove(key);

        if (!registrationRemoved && !stateRemoved)
        {
            return;
        }

        StateChanged?.Invoke();
    }

    private void SetOpen(string key, bool isOpen)
    {
        if (_openStates.TryGetValue(key, out var currentState) && currentState == isOpen)
        {
            return;
        }

        _openStates[key] = isOpen;
        StateChanged?.Invoke();
    }

    private DrawerWidth? GetOpenWidth(KalDrawerSide side, params KalDrawerVariant[] variants)
    {
        return _openStates
            .Where(state => state.Value)
            .Select(state => _registrations.TryGetValue(state.Key, out var registration) ? registration : null)
            .Where(registration =>
                registration is not null
                && registration.Side == side
                && variants.Contains(registration.Variant))
            .Select(registration => registration!.Width)
            .OrderByDescending(width => (int)width)
            .Cast<DrawerWidth?>()
            .FirstOrDefault();
    }

    private sealed record KalDrawerRegistration(KalDrawerSide Side, DrawerWidth Width, KalDrawerVariant Variant);

    private sealed record KalAppBarRegistration(bool Bottom, bool Fixed);
}
