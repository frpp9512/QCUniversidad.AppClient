namespace QCUniversidad.Api.ConfigurationModels;
public record SpecificCalculationValue
{
    public required string Key { get; set; }
    public double Value { get; set; }
}
