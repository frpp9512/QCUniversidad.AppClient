using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.Api.Shared.Extensions;
using QCUniversidad.WebClient.Models.Configuration;
using QCUniversidad.WebClient.Models.LoadDistribution;
using QCUniversidad.WebClient.Models.LoadItem;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Contracts;
using QCUniversidad.WebClient.Services.Platform;
using System.Drawing;

namespace QCUniversidad.WebClient.Controllers;

[Authorize("Distributor")]
public class LoadDistributionController(IDepartmentsDataProvider departmentsDataProvider,
                                        ISchoolYearDataProvider schoolYearDataProvider,
                                        ICoursesDataProvider coursesDataProvider,
                                        IPeriodsDataProvider periodsDataProvider,
                                        IPlanningDataProvider planningDataProvider,
                                        ITeachersDataProvider teachersDataProvider,
                                        IMapper mapper,
                                        IOptions<NavigationSettings> navOptions,
                                        ILogger<PlanningController> logger) : Controller
{
    private readonly IDepartmentsDataProvider _departmentsDataProvider = departmentsDataProvider;
    private readonly ISchoolYearDataProvider _schoolYearDataProvider = schoolYearDataProvider;
    private readonly ICoursesDataProvider _coursesDataProvider = coursesDataProvider;
    private readonly IPeriodsDataProvider _periodsDataProvider = periodsDataProvider;
    private readonly IPlanningDataProvider _planningDataProvider = planningDataProvider;
    private readonly ITeachersDataProvider _teachersDataProvider = teachersDataProvider;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PlanningController> _logger = logger;
    private readonly NavigationSettings _navigationSettings = navOptions.Value;

    [HttpGet]
    public async Task<IActionResult> IndexAsync(Guid? departmentId = null, Guid? schoolYearId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("SelectDepartment");
        }

        if (!User.IsAdmin() && schoolYearId is not null)
        {
            schoolYearId = null;
        }

        Guid workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        Models.SchoolYears.SchoolYearModel schoolYear = schoolYearId is null ? await _schoolYearDataProvider.GetCurrentSchoolYear() : await _schoolYearDataProvider.GetSchoolYearAsync(schoolYearId.Value);
        Models.Departments.DepartmentModel department = await _departmentsDataProvider.GetDepartmentAsync(workingDepartment);
        IList<Models.Course.CourseModel> courses = await _coursesDataProvider.GetCoursesForDepartment(workingDepartment, schoolYear.Id);
        courses = courses.OrderBy(c => c.CareerId).ThenBy(c => c.TeachingModality).ThenBy(c => c.CareerYear).ToList();
        IList<Models.Periods.PeriodModel> periods = await _periodsDataProvider.GetPeriodsAsync(schoolYear.Id);
        LoadDistributionIndexModel model = new()
        {
            Department = department,
            Courses = courses,
            SchoolYear = schoolYear,
            Periods = periods
        };
        return View(model);
    }

    public async Task<IActionResult> WorkForceAsync(Guid? departmentId = null, Guid? schoolYearId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("SelectDepartment", new { redirectTo = "WorkForce" });
        }

        if (!User.IsAdmin() && schoolYearId is not null)
        {
            schoolYearId = null;
        }

        Guid workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        Models.SchoolYears.SchoolYearModel schoolYear = schoolYearId is null ? await _schoolYearDataProvider.GetCurrentSchoolYear() : await _schoolYearDataProvider.GetSchoolYearAsync(schoolYearId.Value);
        Models.Departments.DepartmentModel department = await _departmentsDataProvider.GetDepartmentAsync(workingDepartment);
        IList<Models.Periods.PeriodModel> periods = await _periodsDataProvider.GetPeriodsAsync(schoolYear.Id);
        WorkForceViewModel model = new()
        {
            Department = department,
            Periods = periods,
            SchoolYear = schoolYear
        };
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> SelectDepartmentAsync(string redirectTo = "Index")
    {
        IList<Models.SchoolYears.SchoolYearModel> schoolYears = await _schoolYearDataProvider.GetSchoolYearsAsync();
        IList<Models.Departments.DepartmentModel> departments = await _departmentsDataProvider.GetDepartmentsAsync();
        if (schoolYears.Count == departments.Count && departments.Count == 1)
        {
            return RedirectToAction(redirectTo, new { departmentId = departments.First().Id, schoolYearId = schoolYears.First().Id });
        }

        SelectDepartmentViewModel model = new()
        {
            Departments = departments,
            SchoolYears = schoolYears,
            RedirectTo = redirectTo
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetPeriodOptionsAsync(Guid courseId, Guid? schoolYearId, Guid? departmentId)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }

        _ = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            IList<Models.Periods.PeriodModel> result = await _periodsDataProvider.GetPeriodsAsync(schoolYearId);
            return PartialView("_PeriodOptions", result);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPlanningItemsViewAsync(Guid periodId, Guid? departmentId, Guid? courseId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }

        Guid workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            IList<Models.Planning.TeachingPlanItemModel> result = await _planningDataProvider.GetTeachingPlanItemsOfDepartmentOnPeriodAsync(workingDepartment, periodId, courseId, true);
            result = [.. result.OrderBy(item => item.SubjectId).ThenBy(item => item.Course.CareerId).ThenBy(item => item.Course.CareerYear)];
            return PartialView("_SimplifiedPlanningListView", result);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTeachersForDepartmentInPeriodAsync(Guid periodId, Guid? departmentId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }

        Guid workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            IList<TeacherModel> depTeachers = await _teachersDataProvider.GetTeachersOfDepartmentForPeriodAsync(workingDepartment, periodId);
            IList<TeacherModel> supportTeachers = await _teachersDataProvider.GetSupportTeachersAsync(workingDepartment, periodId);
            TeachersViewModel model = new()
            {
                DepartmentsTeacher = depTeachers,
                SupportTeachers = supportTeachers
            };
            return PartialView("_TeachersWithLoadView", model);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAddLoadModalAsync(Guid planItemId, Guid? departmentId = null, Guid? disciplineId = null)
    {
        if (planItemId == Guid.Empty)
        {
            return BadRequest("The plan item should not be null");
        }

        if (User.IsAdmin() && departmentId is null)
        {
            return BadRequest("If you're logged in as admin you should provide a department id.");
        }

        Guid workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        Models.Planning.TeachingPlanItemModel loadItem = await _planningDataProvider.GetTeachingPlanItemAsync(planItemId);
        IList<TeacherModel> teachers = await _teachersDataProvider.GetTeachersOfDepartmentNotAssignedToLoadItemAsync(workingDepartment, planItemId, disciplineId);

        AddLoadModalModel viewModel = new()
        {
            PlanItem = loadItem,
            Teachers = teachers,
            MaxValue = loadItem.TotalHoursPlanned - loadItem.TotalLoadCovered ?? 0
        };

        return PartialView("_AddLoadModal", viewModel);
    }

    [HttpPut]
    public async Task<IActionResult> SetTeacherLoadAsync([FromBody] CreateLoadItemModel model)
    {
        if (model is not null)
        {
            try
            {
                bool result = await _teachersDataProvider.SetLoadItemAsync(model);
                return result ? Ok(result) : Problem();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        return BadRequest("The model should not be null.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteLoadItemAsync(Guid loadItemId)
    {
        if (loadItemId == Guid.Empty)
        {
            return BadRequest();
        }

        try
        {
            bool result = await _teachersDataProvider.DeleteLoadItemAsync(loadItemId);
            return result ? Ok(result) : Problem();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTeacherLoadDetailsModalContentAsync(Guid teacherId, Guid periodId)
    {
        if (teacherId == Guid.Empty)
        {
            return BadRequest("Should provide a teacher id.");
        }

        if (periodId == Guid.Empty)
        {
            return BadRequest("Should provide a period id.");
        }

        try
        {
            TeacherModel teacher = await _teachersDataProvider.GetTeacherAsync(teacherId, periodId);
            IList<LoadViewItemModel> loadItems = await _teachersDataProvider.GetTeacherLoadItemsInPeriodAsync(teacherId, periodId);
            TeacherLoadViewModel model = new()
            {
                Teacher = teacher,
                LoadItems = loadItems
            };
            return PartialView("_TeacherLoadViewContent", model);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SetNonTeachingLoadAsync(SetNonTeachingLoadModel model)
    {
        if (model is null || !Enum.TryParse(typeof(NonTeachingLoadType), model.Type, out _))
        {
            return BadRequest();
        }

        bool result = await _teachersDataProvider.SetNonTeachingLoadAsync(model);
        return result ? Ok(result) : BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkForceChartDataAsync(Guid periodId, Guid? departmentId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }

        Guid workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            IList<TeacherModel> depTeachers = await _teachersDataProvider.GetTeachersOfDepartmentForPeriodAsync(workingDepartment, periodId);
            ChartModel chartData = ModelListCharter.GetChartModel(ChartType.Bar, depTeachers.OrderBy(t => t.Load.LoadPercent).ToList(), t => t.Load.LoadPercent, t => t.FirstName, title: "Distribución de carga", subtitle: "Comparación de la distribución de cargas entre los profesores del departamento.", showXGrid: false, xScaleTitle: "Profesores del departamento", yScaleTitle: "Carga (%)");
            return Ok(chartData.GetJson());
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkForceTeachingLoadChartDataAsync(Guid periodId, Guid? departmentId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }

        Guid workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            IList<TeacherModel> depTeachers = await _teachersDataProvider.GetTeachersOfDepartmentForPeriodWithLoadItemsAsync(workingDepartment, periodId);
            ChartModel chartData = ModelListCharter.GetChartModel(
                ChartType.Bar,
                depTeachers,
                t => Math.Round(t.LoadViewItems.Where(item => item.Type == LoadViewItemType.Teaching).Sum(item => item.Value) / t.Load.TimeFund * 100),
                t => t.FirstName,
                title: "Distribución de carga",
                subtitle: "Comparación de la distribución de carga directa entre los profesores del departamento.",
                showXGrid: false,
                xScaleTitle: "Profesores del departamento",
                yScaleTitle: "Carga directa (%)");
            return Ok(chartData.GetJson());
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkForceWithMostDirectLoadChartDataAsync(Guid periodId, Guid? departmentId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }

        Guid workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            IList<TeacherModel> depTeachers = await _teachersDataProvider.GetTeachersOfDepartmentForPeriodWithLoadItemsAsync(workingDepartment, periodId);
            depTeachers = depTeachers.OrderBy(t => t.ContractType).ThenByDescending(t => t.SpecificTimeFund).ToList();
            List<(TeacherModel teacher, double directLoad, double indirectLoad)> data = [];
            foreach (TeacherModel teacher in depTeachers)
            {
                IList<LoadViewItemModel>? loadItems = teacher.LoadViewItems;
                if (loadItems is not null)
                {
                    (TeacherModel teacher, double, double) dataValue = (teacher, loadItems.Where(item => item.Type == LoadViewItemType.Teaching).Sum(item => item.Value), loadItems.Where(item => item.Type == LoadViewItemType.NonTeaching).Sum(item => item.Value));
                    data.Add(dataValue);
                }
            }

            IEnumerable<ChartDataValue> dataSet1Values = data.Select(dataItem => new ChartDataValue
            {
                BackgroundColor = ModelListCharter.BackColors[3],
                BorderColor = ModelListCharter.BorderColors[3],
                Value = dataItem.directLoad
            });

            IEnumerable<ChartDataValue> dataSet2Values = data.Select(dataItem => new ChartDataValue
            {
                BackgroundColor = ModelListCharter.BackColors[4],
                BorderColor = ModelListCharter.BorderColors[4],
                Value = dataItem.indirectLoad
            });

            ChartDataEntry dataSet1 = new() { Label = "Carga directa", DataValues = dataSet1Values.ToArray() };
            ChartDataEntry dataSet2 = new() { Label = "Carga indirecta", DataValues = dataSet2Values.ToArray() };

            ChartData chartData = new()
            {
                Labels = data.Select(dataItem => $"{dataItem.teacher.FirstName} {(dataItem.teacher.ContractType == TeacherContractType.PartTime ? $"({dataItem.teacher.ContractType.GetEnumDisplayNameValue()} [{dataItem.teacher.SpecificTimeFund}h/mes])" : "")}").ToArray(),
                DataSets = new ChartDataEntry[] { dataSet2, dataSet1 }
            };

            ChartModel chartModel = new()
            {
                Title = "Distribución de tiempo",
                ShowTitle = true,
                Subtitle = "Vista comparativa de los tiempos de carga de los profesores del departamento",
                ShowSubtitle = true,
                ElementId = "load-distribution-chart",
                LegendPosition = ChartLegendPosition.Bottom,
                Responsive = true,
                ShowXGrid = false,
                ShowYGrid = true,
                Type = ChartType.Bar,
                Stacked = true,
                XScaleTitle = "Profesores del departamento",
                YScaleTitle = "Carga (h/período)",
                Data = chartData
            };

            return Ok(chartModel.GetJson());
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTeachersChartAsync(Guid? departmentId = null)
    {
        IList<TeacherModel> teachers;
        if (User.IsAdmin() && departmentId == null)
        {
            return BadRequest();
        }
        else
        {
            teachers = User.IsAdmin()
                ? await _teachersDataProvider.GetTeachersOfDepartmentAsync(departmentId.Value)
                : await _teachersDataProvider.GetTeachersOfDepartmentAsync(User.GetDepartmentId());
        }

        ChartModel chartModel = ModelListCharter.GetChartModel(ChartType.Doughnut,
                                                        new List<TeacherCategory>((TeacherCategory[])Enum.GetValues(typeof(TeacherCategory))),
                                                        e => teachers.Count(t => t.Category == e),
                                                        e => e.GetEnumDisplayNameValue(),
                                                        title: "Profesores por categoría",
                                                        showXScale: false,
                                                        showYScale: false,
                                                        legendPosition: ChartLegendPosition.Left);
        return Ok(chartModel.GetJson());
    }

    [HttpGet]
    public async Task<IActionResult> GetTeachersChartByContractTypeAsync(Guid? departmentId = null)
    {
        IList<TeacherModel> teachers;
        if (User.IsAdmin() && departmentId == null)
        {
            return BadRequest();
        }
        else
        {
            teachers = User.IsAdmin()
                ? await _teachersDataProvider.GetTeachersOfDepartmentAsync(departmentId.Value)
                : await _teachersDataProvider.GetTeachersOfDepartmentAsync(User.GetDepartmentId());
        }

        ChartModel chartModel = ModelListCharter.GetChartModel(ChartType.Doughnut,
                                                        new List<TeacherContractType>((TeacherContractType[])Enum.GetValues(typeof(TeacherContractType))),
                                                        e => teachers.Count(t => t.ContractType == e),
                                                        e => e.GetEnumDisplayNameValue(),
                                                        title: "Profesores por tipo de contrato",
                                                        showXScale: false,
                                                        showYScale: false,
                                                        legendPosition: ChartLegendPosition.Left);
        return Ok(chartModel.GetJson());
    }

    [HttpGet]
    public async Task<IActionResult> GetTeachersChartByAgeAsync(Guid? departmentId = null)
    {
        IList<TeacherModel> teachers;
        if (User.IsAdmin() && departmentId == null)
        {
            return BadRequest();
        }
        else
        {
            teachers = User.IsAdmin()
                ? await _teachersDataProvider.GetTeachersOfDepartmentAsync(departmentId.Value)
                : await _teachersDataProvider.GetTeachersOfDepartmentAsync(User.GetDepartmentId());
        }

        var ageGroups = new[]
        {
            new
            {
                groupName = "Hasta 30 años",
                value = teachers.Count(t => t.Age <= 30)
            },
            new
            {
                groupName = "De 31 a 40 años",
                value = teachers.Count(t => t.Age is > 30 and <= 40)
            },
            new
            {
                groupName = "De 41 a 50 años",
                value = teachers.Count(t => t.Age is > 40 and <= 50)
            },
            new
            {
                groupName = "De 51 a 60 años",
                value = teachers.Count(t => t.Age is > 50 and <= 60)
            },
            new
            {
                groupName = "De 61 a 65 años",
                value = teachers.Count(t => t.Age is > 60 and <= 65)
            },
            new
            {
                groupName = "Mayores de 65 años",
                value = teachers.Count(t => t.Age > 65)
            }
        };
        ChartModel chartModel = ModelListCharter.GetChartModel(ChartType.Doughnut,
                                                        ageGroups,
                                                        ag => ag.value,
                                                        ag => $"{ag.groupName} ({Math.Round(ag.value / (double)teachers.Count, 2) * 100}%)",
                                                        title: "Profesores rango de edad",
                                                        showXScale: false,
                                                        showYScale: false,
                                                        legendPosition: ChartLegendPosition.Left);
        return Ok(chartModel.GetJson());
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartmentLoadByLoadCategoryAsync(Guid periodId, Guid? departmentId = null)
    {
        if (User.IsAdmin() && departmentId is null)
        {
            return RedirectToAction("Error", "Home");
        }

        Guid workingDepartment = User.IsDepartmentManager() ? User.GetDepartmentId() : departmentId.Value;
        try
        {
            IList<TeacherModel> depTeachers = await _teachersDataProvider.GetTeachersOfDepartmentForPeriodWithLoadItemsAsync(workingDepartment, periodId);
            double total = depTeachers.Sum(t => t.LoadViewItems?.Sum(l => l.Value)) ?? 1;

            List<(string loadCategory, double value)> values =
            [
                ("Formación", Math.Round(depTeachers.Sum(t => t.LoadViewItems.Where(l => l.NonTeachingLoadType is null || l.NonTeachingLoadType?.IsFormationLoad() is true).Sum(l => l.Value)) / total * 100, 2)),
                ("Investigación", Math.Round(depTeachers.Sum(t => t.LoadViewItems.Where(l => l.NonTeachingLoadType?.IsResearchLoad() is true).Sum(l => l.Value)) / total * 100, 2)),
                ("Extensión", Math.Round(depTeachers.Sum(t => t.LoadViewItems.Where(l => l.NonTeachingLoadType?.IsUniversityExtensionLoad() is true).Sum(l => l.Value)) / total * 100, 2)),
                ("Otras actividades", Math.Round(depTeachers.Sum(t => t.LoadViewItems.Where(l => l.NonTeachingLoadType?.IsOthersLoad() is true).Sum(l => l.Value)) / total * 100, 2))
            ];

            ChartModel chartData = ModelListCharter.GetChartModel(
                ChartType.Pie,
                values,
                v => v.value,
                v => v.loadCategory,
                title: "Carga por proceso clave",
                subtitle: "Comparación de la carga en los diferentes procesos claves del departamento.",
                showXGrid: false,
                showYGrid: false,
                showXScale: false,
                showYScale: false,
                legendPosition: ChartLegendPosition.Left,
                xScaleTitle: "Procesos clave",
                yScaleTitle: "Carga (%)");
            return Ok(chartData.GetJson());
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> DownloadTeacherLoadSummaryAsync(Guid teacherId, Guid periodId)
    {
        if (teacherId == Guid.Empty)
        {
            return BadRequest("Should provide a teacher id.");
        }

        if (periodId == Guid.Empty)
        {
            return BadRequest("Should provide a period id.");
        }

        try
        {
            Models.Periods.PeriodModel period = await _periodsDataProvider.GetPeriodAsync(periodId);
            TeacherModel teacher = await _teachersDataProvider.GetTeacherAsync(teacherId, periodId);
            IList<LoadViewItemModel> loadItems = await _teachersDataProvider.GetTeacherLoadItemsInPeriodAsync(teacherId, periodId);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Definier el tipo de licencia, sino da error a la hora de crear el Excel
            using ExcelPackage excel = new(); // Utilizar un using para no tener que hacer dispose al final de las operaciones
            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Carga del profesor");
            worksheet.Cells.Style.Font.Name = "Segoe UI";
            ExcelRange date = worksheet.Cells["D1:E1"];
            date.Merge = true;
            date.Value = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            date.Style.Font.Bold = true;
            date.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            date.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            date.Style.Font.Color.SetColor(Color.FromArgb(100, 92, 92, 184));
            ExcelRange title = worksheet.Cells["A1"];
            title.Value = "Carga del profesor";
            title.Style.Font.Size = 16;
            title.Style.Font.Color.SetColor(Color.FromArgb(100, 92, 92, 184));

            ExcelRange periodDetails = worksheet.Cells["A2:E2"];
            periodDetails["A2"].Value = "Período (Inicia | Termina | Meses | Fondo tiempo)";
            periodDetails["B2"].Value = period.Starts.ToString("dd-MM-yyyy");
            periodDetails["C2"].Value = period.Ends.ToString("dd-MM-yyyy");
            periodDetails["D2"].Value = $"{period.MonthsCount} meses";
            periodDetails["E2"].Value = $"{period.TimeFund} h";

            ExcelRange teacherName = worksheet.Cells["A3:E3"];
            ExcelRange teacherNameTitle = teacherName["A3"];
            teacherNameTitle.Value = "Nombre completo";
            ExcelRange teacherNameValue = teacherName["B3:E3"];
            teacherNameValue.Merge = true;
            teacherNameValue.Value = teacher.Fullname;

            ExcelRange teacherPersonalId = worksheet.Cells["A4:E4"];
            teacherPersonalId["A4"].Value = "Carné de identidad";
            ExcelRange teacherPersonalIdValue = teacherPersonalId["B4:E4"];
            teacherPersonalIdValue.Merge = true;
            teacherPersonalIdValue.Value = teacher.PersonalId;

            ExcelRange teacherCategory = worksheet.Cells["A5:E5"];
            teacherCategory["A5"].Value = "Categoría";
            ExcelRange teacherCategoryValue = teacherCategory["B5:E5"];
            teacherCategoryValue.Merge = true;
            teacherCategoryValue.Value = teacher.Category.GetEnumDisplayNameValue();

            ExcelRange teacherContractType = worksheet.Cells["A6:E6"];
            teacherContractType["A6"].Value = "Tipo de contrato";
            ExcelRange teacherContractTypeValue = teacherCategory["B6:E6"];
            teacherContractTypeValue.Merge = true;
            teacherContractTypeValue.Value = $"{teacher.ContractType.GetEnumDisplayNameValue()}{(teacher.ContractType == TeacherContractType.PartTime ? $" ({teacher.SpecificTimeFund} h/mes)" : "")}";

            ExcelRange teacherPosition = worksheet.Cells["A7:E7"];
            teacherPosition["A7"].Value = "Cargo";
            ExcelRange teacherPositionValue = teacherCategory["B7:E7"];
            teacherPositionValue.Merge = true;
            teacherPositionValue.Value = teacher.Position;

            ExcelRange teacherEmail = worksheet.Cells["A8:E8"];
            teacherEmail["A8"].Value = "Correo electrónico";
            ExcelRange teacherEmailValue = worksheet.Cells["B8:E8"];
            teacherEmailValue.Merge = true;
            teacherEmailValue.Value = teacher.Email;

            ExcelRange teacherBirthdate = worksheet.Cells["A9:E9"];
            teacherBirthdate["A9"].Value = "Fecha de nacimiento";
            ExcelRange teacherBirthdateValue = worksheet.Cells["B9:E9"];
            teacherBirthdateValue.Merge = true;
            teacherBirthdateValue.Value = teacher.Birthday?.ToShortDateString();

            ExcelRange teacherAge = worksheet.Cells["A10:E10"];
            teacherAge["A10"].Value = "Edad";
            ExcelRange teacherAgeValue = worksheet.Cells["B10:E10"];
            teacherAgeValue.Merge = true;
            teacherAgeValue.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            teacherAgeValue.Value = teacher.Age;

            ExcelRange teacherLoad = worksheet.Cells["A11:D11"];
            teacherLoad["A11"].Value = "Carga (Carga total | Fondo tiempo | %)";
            double loadtotal = loadItems.Sum(l => l.Value);
            teacherLoad["B11"].Value = loadtotal;

            double timeFund = period.TimeFund;
            if (teacher.ContractType == TeacherContractType.PartTime)
            {
                timeFund = teacher.SpecificTimeFund;
            }

            double loadPercent = Math.Round(loadtotal / timeFund * 100, 2);
            teacherLoad["C11"].Value = timeFund;
            ExcelRange percentCell = teacherLoad["D11"];
            teacherLoad["D11"].Value = loadPercent;

            // Creando la cabecera de la tabla
            List<string> columnHeaders = ["Tipo de carga", "Proceso clave", "Denominación", "Descripción", "Valor"];

            string headerRange = $"A13:{char.ConvertFromUtf32(columnHeaders.Count + 64)}13"; // Rango de la cabecera desde A10:(calcular letra en función de la cantidad de colmnas)10
            ExcelRangeBase header = worksheet.Cells[headerRange].LoadFromArrays(new List<string[]> { columnHeaders.ToArray() }); // Agregamos la información de la cabecera a la hora de trabajo y seleccionamos en rango
                                                                                                                                 // Dar formato al rango de la cabecera
            header.Style.Font.Bold = true;
            header.Style.Font.Color.SetColor(Color.White);
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(100, 92, 92, 184));

            // Cargar los datos del cuerpo de la tabla
            List<object?[]> bodyData = [];
            foreach (LoadViewItemModel value in loadItems)
            {
                List<object?> row =
                [
                    value.Type.GetEnumDisplayNameValue(),
                    value.NonTeachingLoadType is null ? NonTeachingLoadType.ClassPreparation.GetNonTeachingLoadCategoryPromtName() : value.NonTeachingLoadType?.GetNonTeachingLoadCategoryPromtName(),
                    value.Type == LoadViewItemType.NonTeaching ? value.NonTeachingLoadType?.GetEnumDisplayNameValue() : "Docencia directa",
                    value.Description,
                    value.Value
                ];
                bodyData.Add(row.ToArray());
            }

            // Agregar los datos de la tabla a partir de la segunda fila
            ExcelRangeBase body = worksheet.Cells[14, 1].LoadFromArrays(bodyData);
            body.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            body.AutoFitColumns(18, 75);

            ExcelRange descriptions = worksheet.Cells[$"D14:D{loadItems.Count + 14}"];
            descriptions.Style.WrapText = true;

            ExcelRange teacherDetailTitles = worksheet.Cells["A2:A11"];
            teacherDetailTitles.Style.Font.Bold = true;
            teacherDetailTitles.Style.Font.Color.SetColor(Color.FromArgb(100, 92, 92, 184));
            teacherDetailTitles.AutoFitColumns();

            ExcelRange devInfo = worksheet.Cells[$"A{14 + loadItems.Count + 2}:E{14 + loadItems.Count + 2}"];
            devInfo.Merge = true;
            devInfo.Value = $"QCUniversidad - Frank Raúl Pérez Pérez © 2022";
            devInfo.Style.Font.Color.SetColor(Color.FromArgb(100, 92, 92, 184));
            devInfo.Style.Font.Bold = true;
            devInfo.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Crear la tabla de excel para que aplique toda
            ExcelRange tableRange = worksheet.Cells[$"A13:E{13 + loadItems.Count}"];
            OfficeOpenXml.Table.ExcelTable table = worksheet.Tables.Add(tableRange, "Carga");
            table.ShowRowStripes = true;
            table.ShowTotal = true;

            // Ajustes de página para impresión
            worksheet.PrinterSettings.HorizontalCentered = true;
            worksheet.PrinterSettings.VerticalCentered = true;
            worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
            worksheet.PrinterSettings.PaperSize = ePaperSize.Letter;
            worksheet.PrinterSettings.FitToPage = true;

            byte[] array = await excel.GetAsByteArrayAsync();
            // Retornar el FileStream con el array del archivo Excel
            return new FileStreamResult(new MemoryStream(array), new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
            {
                FileDownloadName = $"Carga de {teacher.Fullname}.xlsx"
            };
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartmentPeriodStatsAsync(Guid departmentId, Guid periodId)
    {
        if (departmentId == Guid.Empty)
        {
            return BadRequest(new { error = "Debe de especificar un id de departamento válido." });
        }

        if (periodId == Guid.Empty)
        {
            return BadRequest(new { error = "Debe de especificar un id de período válido." });
        }

        try
        {
            IList<Models.Statistics.StatisticItemModel> stats = await _departmentsDataProvider.GetDepartmentPeriodStatsAsync(departmentId, periodId);
            return PartialView("_DepartmentPeriodStats", stats);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }
}