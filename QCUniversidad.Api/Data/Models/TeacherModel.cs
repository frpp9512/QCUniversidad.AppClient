using QCUniversidad.Api.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QCUniversidad.Api.Data.Models;

/// <summary>
/// Represents a person that teach a set of subjects from a determined discipline.
/// </summary>
public record TeacherModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The full name of the teacher.
    /// </summary>
    public string? Fullname { get; set; }

    /// <summary>
    /// The identity card number.
    /// </summary>
    [MaxLength(11), MinLength(11)]
    public string? PersonalId { get; set; }

    /// <summary>
    /// The birthdate of the teacher.
    /// </summary>
    [NotMapped]
    public DateTime? Birthday
    {
        get
        {
            if (!string.IsNullOrEmpty(PersonalId))
            {
                var currentYearSection = DateTime.Now.Year.ToString().Substring(2, 2);
                var yearSection = PersonalId.Substring(0, 2);
                var monthSection = PersonalId.Substring(2, 2);
                var daySection = PersonalId.Substring(4, 2);
                var currentYearSectionValue = int.Parse(currentYearSection);
                var yearSectionValue = int.Parse(yearSection);
                var birthDayYear = currentYearSectionValue < yearSectionValue ? int.Parse($"19{yearSectionValue}") : int.Parse($"20{yearSectionValue}");
                var monthSectionValue = int.Parse(monthSection);
                var daySectionValue = int.Parse(daySection);
                return new DateTime(birthDayYear, monthSectionValue, daySectionValue);
            }
            return null;
        }
    }

    /// <summary>
    /// The current age of the teacher.
    /// </summary>
    [NotMapped]
    public int Age
    {
        get 
        {
            if (Birthday is not null)
            {
                var diff = DateTime.Now - Birthday.Value;
                var years = diff.TotalDays / 365;
                return (int)years;
            }
            return -1;
        }
    }

    /// <summary>
    /// The position that occupies in the university.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// The category of the teacher.
    /// </summary>
    public TeacherCategory Category { get; set; }

    /// <summary>
    /// The type of contract of the teacher.
    /// </summary>
    public TeacherContractType ContractType { get; set; }

    /// <summary>
    /// The email address of the teacher.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The id department which the teacher belongs to.
    /// </summary>
    public Guid DepartmentId { get; set; }

    /// <summary>
    /// The department which the teacher belongs to.
    /// </summary>
    public DepartmentModel? Department { get; set; }

    /// <summary>
    /// The disciplines whose the teacher can teach.
    /// </summary>
    public IList<TeacherDiscipline>? TeacherDisciplines { get; set; }

    /// <summary>
    /// The load items assigned to the teacher.
    /// </summary>
    public IList<LoadItemModel>? LoadItems { get; set; }

    /// <summary>
    /// The set of non-teaching load of the teacher.
    /// </summary>
    public IList<NonTeachingLoadModel>? NonTeachingLoadItems { get; set; }

    /// <summary>
    /// Defines if the teacher is active for generate load distribution.
    /// </summary>
    public bool Active { get; set; } = true;
}