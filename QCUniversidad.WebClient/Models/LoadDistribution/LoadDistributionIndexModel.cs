﻿using QCUniversidad.WebClient.Models.Courses;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.Models.LoadDistribution;

public class LoadDistributionIndexModel
{
    public SchoolYearModel? SchoolYear { get; set; }
    public IList<PeriodModel>? Periods { get; set; }
    public DepartmentModel? Department { get; set; }
    public IList<CourseModel>? Courses { get; set; }
}
