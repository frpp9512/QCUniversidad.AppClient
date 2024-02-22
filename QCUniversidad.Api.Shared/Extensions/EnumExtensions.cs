using QCUniversidad.Api.Shared.Attributes;
using QCUniversidad.Api.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetPlanItemTypeDisplayValue(this TeachingActivityType type)
    {
        Type enumType = type.GetType();
        System.Reflection.MemberInfo[] member = enumType.GetMember(type.ToString());
        System.Reflection.MemberInfo? enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        object[]? attributes = enumMember?.GetCustomAttributes(typeof(DisplayAttribute), false);
        string? display = attributes.Any() ? ((DisplayAttribute)attributes.First()).GetName() : type.ToString();
        return display;
    }

    public static string GetEnumDisplayNameValue<T>(this T enumValue) where T : Enum
    {
        Type enumType = enumValue.GetType();
        System.Reflection.MemberInfo[] member = enumType.GetMember(enumValue.ToString());
        System.Reflection.MemberInfo? enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        object[] attributes = enumMember.GetCustomAttributes(typeof(DisplayAttribute), false);
        string? display = attributes.Any() ? ((DisplayAttribute)attributes.First()).GetName() : enumValue.ToString();
        return display;
    }

    public static string GetEnumDisplayDescriptionValue<T>(this T enumValue) where T : Enum
    {
        Type enumType = enumValue.GetType();
        System.Reflection.MemberInfo[] member = enumType.GetMember(enumValue.ToString());
        System.Reflection.MemberInfo? enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        object[] attributes = enumMember.GetCustomAttributes(typeof(DisplayAttribute), false);
        string? display = attributes.Any() ? ((DisplayAttribute)attributes.First()).GetDescription() : enumValue.ToString();
        return display;
    }

    public static int GetEnumDisplayOrderValue<T>(this T enumValue) where T : Enum
    {
        Type enumType = enumValue.GetType();
        System.Reflection.MemberInfo[] member = enumType.GetMember(enumValue.ToString());
        System.Reflection.MemberInfo? enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        object[] attributes = enumMember.GetCustomAttributes(typeof(DisplayAttribute), false);
        int? order = attributes.Any() ? ((DisplayAttribute)attributes.First()).GetOrder() : 0;
        return order ?? 0;
    }

    public static bool GetEnumDisplayAutogenerateValue<T>(this T enumValue) where T : Enum
    {
        Type enumType = enumValue.GetType();
        System.Reflection.MemberInfo[] member = enumType.GetMember(enumValue.ToString());
        System.Reflection.MemberInfo? enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        object[]? attributes = enumMember?.GetCustomAttributes(typeof(DisplayAttribute), false);
        bool display = attributes.Any() && ((DisplayAttribute)attributes.First()).GetAutoGenerateField() == true;
        return display;
    }

    public static bool IsRecalculable<T>(this T enumValue) where T : Enum
    {
        Type enumType = enumValue.GetType();
        System.Reflection.MemberInfo[] member = enumType.GetMember(enumValue.ToString());
        System.Reflection.MemberInfo? enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        object[]? attributes = enumMember?.GetCustomAttributes(typeof(RecalculateAttribute), false);
        return attributes.Any();
    }

    public static string? GetNonTeachingLoadCategory(this NonTeachingLoadType loadType)
    {
        Type enumType = loadType.GetType();
        System.Reflection.MemberInfo[] member = enumType.GetMember(loadType.ToString());
        System.Reflection.MemberInfo? enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        object[]? attributes = enumMember?.GetCustomAttributes(typeof(LoadCategoryAttribute), false);
        if (attributes?.Length is 0)
        {
            return null;
        }

        string value = ((LoadCategoryAttribute)attributes.First()).Category;
        return value;
    }

    public static string? GetNonTeachingLoadCategoryPromtName(this NonTeachingLoadType loadType)
    {
        Type enumType = loadType.GetType();
        System.Reflection.MemberInfo[] member = enumType.GetMember(loadType.ToString());
        System.Reflection.MemberInfo? enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        object[]? attributes = enumMember?.GetCustomAttributes(typeof(LoadCategoryAttribute), false);
        if (attributes?.Length is 0)
        {
            return null;
        }

        string? value = ((LoadCategoryAttribute)attributes.First()).PromptName;
        return value;
    }

    public static string? GetNonTeachingLoadCategoryDescription(this NonTeachingLoadType loadType)
    {
        Type enumType = loadType.GetType();
        System.Reflection.MemberInfo[] member = enumType.GetMember(loadType.ToString());
        System.Reflection.MemberInfo? enumMember = member.FirstOrDefault(m => m.DeclaringType == enumType);
        object[]? attributes = enumMember?.GetCustomAttributes(typeof(LoadCategoryAttribute), false);
        if (attributes?.Length is 0)
        {
            return null;
        }

        string? value = ((LoadCategoryAttribute)attributes.First()).Description;
        return value;
    }

    public static bool IsResearchLoad(this NonTeachingLoadType loadType) => GetNonTeachingLoadCategory(loadType) == "Research";

    public static bool IsOthersLoad(this NonTeachingLoadType loadType) => GetNonTeachingLoadCategory(loadType) == "Others";

    public static bool IsUniversityExtensionLoad(this NonTeachingLoadType loadType) => GetNonTeachingLoadCategory(loadType) == "UniversityExtension";

    public static bool IsFormationLoad(this NonTeachingLoadType loadType) => GetNonTeachingLoadCategory(loadType) == "Formation";
}