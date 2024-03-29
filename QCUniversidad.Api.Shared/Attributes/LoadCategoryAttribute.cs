﻿namespace QCUniversidad.Api.Shared.Attributes;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public class LoadCategoryAttribute : Attribute
{
    public required string Category { get; set; }
    public string? PromptName { get; set; }
    public string? Description { get; set; }
}