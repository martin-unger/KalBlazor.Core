using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public abstract class BaseComponent : ComponentBase
{
    /// <summary>
    /// Replaces the component default classes.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Additional classes appended to the effective component classes.
    /// </summary>
    [Parameter]
    public string? AdditionalClass { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected virtual string ComponentClass => string.Empty;

    protected abstract string DefaultClass { get; }

    protected virtual string DynamicClass => string.Empty;

    protected IReadOnlyDictionary<string, object>? FilteredAdditionalAttributes =>
        AdditionalAttributes?
            .Where(attribute => !IsClassAttribute(attribute.Key))
            .ToDictionary(StringComparer.OrdinalIgnoreCase);

    protected string CssClass
    {
        get
        {
            var explicitClass = !string.IsNullOrWhiteSpace(Class)
                ? Class
                : TryGetAttributeClass(out var attributeClass)
                    ? attributeClass
                    : string.Empty;

            var effectiveClass = !string.IsNullOrWhiteSpace(explicitClass)
                ? explicitClass
                : $"{DefaultClass} {DynamicClass}".Trim();

            return $"{ComponentClass} {effectiveClass} {AdditionalClass}".Trim();
        }
    }

    private bool TryGetAttributeClass(out string attributeClass)
    {
        attributeClass = string.Empty;

        if (AdditionalAttributes is null)
        {
            return false;
        }

        foreach (var attribute in AdditionalAttributes)
        {
            if (!IsClassAttribute(attribute.Key))
            {
                continue;
            }

            attributeClass = attribute.Value?.ToString() ?? string.Empty;
            return !string.IsNullOrWhiteSpace(attributeClass);
        }

        return false;
    }

    private static bool IsClassAttribute(string attributeName)
    {
        return string.Equals(attributeName, "class", StringComparison.OrdinalIgnoreCase)
            || string.Equals(attributeName, "@class", StringComparison.OrdinalIgnoreCase);
    }
}
