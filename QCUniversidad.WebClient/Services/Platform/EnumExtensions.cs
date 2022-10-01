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
                TeachingModality.Classroom => "Presencial",
                TeachingModality.ByMeeting => "Por encuentro",
                TeachingModality.DistanceLearning => "A distancia",
                _ => throw new NotImplementedException()
            };
    }
}