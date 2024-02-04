namespace QCUniversidad.Api.Services;

public class FacultyNotFoundException : Exception { }
public class DepartmentNotFoundException : Exception { }
public class CareerNotFoundException : Exception { }
public class DisciplineNotFoundException : Exception { }
public class TeacherNotFoundException : Exception { }
public class SubjectNotFoundException : Exception { }
public class CurriculumNotFoundException : Exception { }
public class NotCurrentSchoolYearDefined : Exception { }
public class SchoolYearNotFoundException : Exception { }
public class CourseNotFoundException : Exception { }
public class PeriodNotFoundException : Exception { }
public class TeachingPlanNotFoundException : Exception { }
public class TeachingPlanItemNotFoundException : Exception { }
public class PlanItemFullyCoveredException : Exception { }
public class LoadItemNotFoundException : Exception { }
public class PeriodSubjectNotFoundException : Exception { }
public class DatabaseOperationException : Exception { }
public class NonTeachingLoadUnsettableException : Exception { }
public class ConfigurationException : Exception { }