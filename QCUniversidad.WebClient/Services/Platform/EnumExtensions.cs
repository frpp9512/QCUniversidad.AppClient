using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Services.Platform
{
    public static class EnumExtensions
    {
        public static string GetLabel(this TeachingModality modality) 
            => modality switch
            {
                TeachingModality.Classroom => "Diurno",
                TeachingModality.ByMeeting => "Por encuentro",
                TeachingModality.DistanceLearning => "A distancia",
                TeachingModality.PostgraduateDegree => "Postgrado",
                TeachingModality.MastersDegree => "Maestría",
                TeachingModality.PhDDegree => "Doctorado",
                _ => throw new NotImplementedException()
            };
    }
}