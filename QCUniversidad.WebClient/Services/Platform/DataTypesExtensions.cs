using QCUniversidad.WebClient.Models.SchoolYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Services.Platform
{
    public static class DataTypesExtensions
    {
        public static string GetCareerYearLabel(this SchoolYearModel model)
            => model.CareerYear switch 
            { 
                1 => "1er año",
                2 => "2do año",
                3 => "3er año",
                4 => "4to año",
                5 => "5to año",
                6 => "6to año",
                7 => "7mo año",
                8 => "8vo año",
                _ => throw new NotImplementedException()
            };

        public static Guid GetDepartmentId(this ClaimsPrincipal user)
            => new(user.Claims.First(c => c.Type == "DepartmentId").Value);
    }
}