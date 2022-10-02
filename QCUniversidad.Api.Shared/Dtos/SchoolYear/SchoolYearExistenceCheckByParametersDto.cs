using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Dtos.SchoolYear
{
    public record SchoolYearExistenceCheckByParametersDto
    {
        public Guid CareerId { get; set; }
        public int CareerYear { get; set; }
        public TeachingModality Modality { get; set; }
    }
}
