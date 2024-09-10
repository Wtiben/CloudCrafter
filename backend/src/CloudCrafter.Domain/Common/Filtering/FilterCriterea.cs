using System.Text.Json.Serialization;

namespace CloudCrafter.Domain.Common.Filtering;

public class FilterCriterea
{
    public required string PropertyName { get; set; }

    public required FilterOperatorOption Operator { get; set; }
    public string? Value { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FilterOperatorOption
{
    Equal,
    NotEqual,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    Contains,
}
