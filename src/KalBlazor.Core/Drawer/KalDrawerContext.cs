namespace SoftwareThingies.KalBlazor.Core;

internal sealed class KalDrawerContext
{
    private readonly Dictionary<string, KalDrawerState> _states = new(StringComparer.Ordinal);
    private readonly Dictionary<string, KalDrawerRegistration> _registrations = new(StringComparer.Ordinal);
    private readonly Dictionary<Guid, KalAppBarRegistration> _appBars = [];

    public event Action? StateChanged;

    public event Action<string>? DrawerStateChanged;

    public bool AppBarBottom => _appBars.Values.Any(appBar => appBar.Bottom);

    public bool HasAppBar => _appBars.Count > 0;

    public bool HasFixedTopAppBar => _appBars.Values.Any(appBar => appBar.Fixed && !appBar.Bottom);

    public bool HasFixedBottomAppBar => _appBars.Values.Any(appBar => appBar.Fixed && appBar.Bottom);

    public bool HasOpenDrawers => _states.Values.Any(state => state == KalDrawerState.Maximized);

    public bool HasOpenOverlayDrawers =>
        _states.Any(state =>
            state.Value == KalDrawerState.Maximized
            && _registrations.TryGetValue(state.Key, out var registration)
            && registration.Variant == KalDrawerVariant.Overlay);

    public KalDrawerBackdrop? ActiveBackdrop =>
        _states
            .Where(state => state.Value == KalDrawerState.Maximized)
            .Select(state => _registrations.TryGetValue(state.Key, out var registration) ? registration : null)
            .Where(registration => registration is not null && registration.Backdrop != KalDrawerBackdrop.None)
            .Select(registration => registration!.Backdrop)
            .OrderBy(backdrop => backdrop)
            .Cast<KalDrawerBackdrop?>()
            .FirstOrDefault();

    public bool HasOpenOverlayDrawerWithBackdrop =>
        _states.Any(state =>
            state.Value == KalDrawerState.Maximized
            && _registrations.TryGetValue(state.Key, out var registration)
            && registration.Variant == KalDrawerVariant.Overlay
            && registration.Backdrop != KalDrawerBackdrop.None);

    public KalDrawerOffset? LeftDockedOffset => GetVisibleOffset(KalDrawerSide.Left, KalDrawerVariant.Docked);

    public KalDrawerOffset? RightDockedOffset => GetVisibleOffset(KalDrawerSide.Right, KalDrawerVariant.Docked);

    public KalDrawerOffset? LeftContentOffset => GetVisibleOffset(KalDrawerSide.Left, KalDrawerVariant.Docked, KalDrawerVariant.Clipped);

    public KalDrawerOffset? RightContentOffset => GetVisibleOffset(KalDrawerSide.Right, KalDrawerVariant.Docked, KalDrawerVariant.Clipped);

    public bool IsOpen(string key)
    {
        return GetState(key) == KalDrawerState.Maximized;
    }

    public bool IsMinimized(string key)
    {
        return GetState(key) == KalDrawerState.Minimized;
    }

    public void Open(string key)
    {
        SetState(key, KalDrawerState.Maximized);
    }

    public void Close(string key)
    {
        if (PreventsClose(key))
        {
            return;
        }

        if (MinimizesOnClose(key))
        {
            Minimize(key);
            return;
        }

        SetState(key, KalDrawerState.Hidden);
    }

    public void Minimize(string key)
    {
        if (!IsOpen(key)
            || !_registrations.TryGetValue(key, out var registration)
            || !registration.Minimizable)
        {
            return;
        }

        SetState(key, KalDrawerState.Minimized);
    }

    public void Maximize(string key)
    {
        if (IsMinimized(key))
        {
            Open(key);
        }
    }

    public void ToggleMinimized(string key)
    {
        if (IsMinimized(key))
        {
            Maximize(key);
            return;
        }

        Minimize(key);
    }

    public void Toggle(string key)
    {
        if (IsOpen(key))
        {
            Close(key);
            return;
        }

        Open(key);
    }

    public void CloseAll()
    {
        var changed = false;

        foreach (var key in _states.Keys.ToArray())
        {
            if (_states[key] == KalDrawerState.Hidden || PreventsClose(key))
            {
                continue;
            }

            var targetState = MinimizesOnClose(key)
                ? KalDrawerState.Minimized
                : KalDrawerState.Hidden;

            if (_states[key] == targetState)
            {
                continue;
            }

            _states[key] = targetState;
            DrawerStateChanged?.Invoke(key);
            changed = true;
        }

        if (changed)
        {
            StateChanged?.Invoke();
        }
    }

    public void Register(
        string key,
        KalDrawerSide side,
        DrawerWidth width,
        KalDrawerVariant variant,
        KalDrawerBackdrop backdrop,
        bool preventClose,
        bool minimizable,
        bool minimizeOnClose,
        string minimizedOffsetClass)
    {
        var registration = new KalDrawerRegistration(
            side,
            width,
            variant,
            backdrop,
            preventClose,
            minimizable,
            minimizeOnClose,
            minimizedOffsetClass);

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
        var stateRemoved = _states.Remove(key);

        if (!registrationRemoved && !stateRemoved)
        {
            return;
        }

        StateChanged?.Invoke();
    }

    public KalDrawerState GetState(string key)
    {
        return _states.TryGetValue(key, out var state) ? state : KalDrawerState.Hidden;
    }

    public void SetState(string key, KalDrawerState state)
    {
        SetStateCore(key, state);
    }

    private void SetStateCore(string key, KalDrawerState state)
    {
        if (_states.TryGetValue(key, out var currentState) && currentState == state)
        {
            return;
        }

        _states[key] = state;
        DrawerStateChanged?.Invoke(key);
        StateChanged?.Invoke();
    }

    private bool PreventsClose(string key)
    {
        return _registrations.TryGetValue(key, out var registration) && registration.PreventClose;
    }

    private bool MinimizesOnClose(string key)
    {
        return _registrations.TryGetValue(key, out var registration) && registration.MinimizeOnClose;
    }

    private KalDrawerOffset? GetVisibleOffset(KalDrawerSide side, params KalDrawerVariant[] variants)
    {
        return _states
            .Where(state => state.Value != KalDrawerState.Hidden)
            .Select(state => _registrations.TryGetValue(state.Key, out var registration)
                ? new { state.Value, Registration = registration }
                : null)
            .Where(item =>
                item is not null
                && item.Registration.Side == side
                && variants.Contains(item.Registration.Variant))
            .OrderByDescending(item =>
                item!.Value == KalDrawerState.Maximized
                    ? (int)item.Registration.Width + 1
                    : 0)
            .Select(item =>
                item!.Value == KalDrawerState.Minimized
                    ? new KalDrawerOffset(null, item.Registration.MinimizedOffsetClass)
                    : new KalDrawerOffset(item.Registration.Width, null))
            .FirstOrDefault();
    }

    private sealed record KalDrawerRegistration(
        KalDrawerSide Side,
        DrawerWidth Width,
        KalDrawerVariant Variant,
        KalDrawerBackdrop Backdrop,
        bool PreventClose,
        bool Minimizable,
        bool MinimizeOnClose,
        string MinimizedOffsetClass);

    private sealed record KalAppBarRegistration(bool Bottom, bool Fixed);
}
