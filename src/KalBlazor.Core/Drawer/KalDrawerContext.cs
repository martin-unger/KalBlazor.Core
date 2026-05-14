namespace SoftwareThingies.KalBlazor.Core;

internal sealed class KalDrawerContext
{
    private readonly Dictionary<string, bool> _openStates = new(StringComparer.Ordinal);
    private readonly Dictionary<string, KalDrawerRegistration> _registrations = new(StringComparer.Ordinal);
    private int _appBarCount;
    private bool _appBarBottom;

    public event Action? StateChanged;

    public bool AppBarBottom => _appBarBottom;

    public bool HasAppBar => _appBarCount > 0;

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

    public void RegisterAppBar()
    {
        _appBarCount++;
        StateChanged?.Invoke();
    }

    public void SetAppBarPosition(bool bottom)
    {
        if (_appBarBottom == bottom)
        {
            return;
        }

        _appBarBottom = bottom;
        StateChanged?.Invoke();
    }

    public void UnregisterAppBar()
    {
        if (_appBarCount == 0)
        {
            return;
        }

        _appBarCount--;
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
}
