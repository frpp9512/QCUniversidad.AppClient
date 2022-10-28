namespace QCUniversidad.Api.Shared.Attributes;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public sealed class RecalculateAttribute : Attribute
{
    public RecalculateAttribute()
    {
    }
}