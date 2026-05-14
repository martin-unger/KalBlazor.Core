using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public abstract class BaseComponent : ComponentBase
{
    /// <summary>
    /// This class overrides the default class.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected abstract string DefaultClass { get; }

    protected virtual string AdditionalClass => string.Empty;

    protected IReadOnlyDictionary<string, object>? FilteredAdditionalAttributes =>
        AdditionalAttributes?
            .Where(attribute => !IsClassAttribute(attribute.Key))
            .ToDictionary(StringComparer.OrdinalIgnoreCase);

    protected string CssClass
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Class))
            {
                return Class;
            }

            return TryGetAttributeClass(out var attributeClass) ? attributeClass : $"{DefaultClass} {AdditionalClass}".Trim();
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
