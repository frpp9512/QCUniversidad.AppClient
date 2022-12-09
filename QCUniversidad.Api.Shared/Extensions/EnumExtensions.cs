using QCUniversidad.Api.Shared.Attributes;
using QCUniversidad.Api.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetPlanItemTypeDisplayValue(this TeachingActivityType type)
    {
        var enumType = type.GetType();
        var member = enumType.GetMember(type.ToString());
        var enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        var attributes = enumMember?.GetCustomAttributes(typeof(DisplayAttribute), false);
        var display = attributes.Any() ? ((DisplayAttribute)attributes.First()).GetName() : type.ToString();
        return display;
    }

    public static string GetEnumDisplayNameValue<T>(this T enumValue) where T : Enum
    {
        var enumType = enumValue.GetType();
        var member = enumType.GetMember(enumValue.ToString());
        var enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        var attributes = enumMember.GetCustomAttributes(typeof(DisplayAttribute), false);
        var display = attributes.Any() ? ((DisplayAttribute)attributes.First()).GetName() : enumValue.ToString();
        return display;
    }

    public static string GetEnumDisplayDescriptionValue<T>(this T enumValue) where T : Enum
    {
        var enumType = enumValue.GetType();
        var member = enumType.GetMember(enumValue.ToString());
        var enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        var attributes = enumMember.GetCustomAttributes(typeof(DisplayAttribute), false);
        var display = attributes.Any() ? ((DisplayAttribute)attributes.First()).GetDescription() : enumValue.ToString();
        return display;
    }

    public static int GetEnumDisplayOrderValue<T>(this T enumValue) where T : Enum
    {
        var enumType = enumValue.GetType();
        var member = enumType.GetMember(enumValue.ToString());
        var enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        var attributes = enumMember.GetCustomAttributes(typeof(DisplayAttribute), false);
        var order = attributes.Any() ? ((DisplayAttribute)attributes.First()).GetOrder() : 0;
        return order ?? 0;
    }

    public static bool GetEnumDisplayAutogenerateValue<T>(this T enumValue) where T : Enum
    {
        var enumType = enumValue.GetType();
        var member = enumType.GetMember(enumValue.ToString());
        var enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        var attributes = enumMember?.GetCustomAttributes(typeof(DisplayAttribute), false);
        var display = attributes.Any() && ((DisplayAttribute)attributes.First()).GetAutoGenerateField() == true;
        return display;
    }

    public static bool IsRecalculable<T>(this T enumValue) where T : Enum
    {
        var enumType = enumValue.GetType();
        var member = enumType.GetMember(enumValue.ToString());
        var enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        var attributes = enumMember?.GetCustomAttributes(typeof(RecalculateAttribute), false);
        return attributes.Any();
    }

    public static string? GetNonTeachingLoadCategory(this NonTeachingLoadType loadType)
    {
        var enumType = loadType.GetType();
        var member = enumType.GetMember(loadType.ToString());
        var enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        var attributes = enumMember?.GetCustomAttributes(typeof(LoadCategoryAttribute), false);
        if (attributes?.Any() is true)
        {
            var value = ((LoadCategoryAttribute)attributes.First()).Category;
            return value;
        }
        return null;
    }

    public static string? GetNonTeachingLoadCategoryPromtName(this NonTeachingLoadType loadType)
    {
        var enumType = loadType.GetType();
        var member = enumType.GetMember(loadType.ToString());
        var enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        var attributes = enumMember?.GetCustomAttributes(typeof(LoadCategoryAttribute), false);
        if (attributes?.Any() is true)
        {
            var value = ((LoadCategoryAttribute)attributes.First()).PromtName;
            return value;
        }
        return null;
    }

    public static string? GetNonTeachingLoadCategoryDescription(this NonTeachingLoadType loadType)
    {
        var enumType = loadType.GetType();
        var member = enumType.GetMember(loadType.ToString());
        var enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        var attributes = enumMember?.GetCustomAttributes(typeof(LoadCategoryAttribute), false);
        if (attributes?.Any() is true)
        {
            var value = ((LoadCategoryAttribute)attributes.First()).Description;
            return value;
        }
        return null;
    }

    public static bool IsResearchLoad(this NonTeachingLoadType loadType)
        => GetNonTeachingLoadCategory(loadType) == "Research";

    public static bool IsOthersLoad(this NonTeachingLoadType loadType)
        => GetNonTeachingLoadCategory(loadType) == "Others";

    public static bool IsUniversityExtensionLoad(this NonTeachingLoadType loadType)
        => GetNonTeachingLoadCategory(loadType) == "UniversityExtension";

    public static bool IsFormationLoad(this NonTeachingLoadType loadType)
        => GetNonTeachingLoadCategory(loadType) == "Formation";
}