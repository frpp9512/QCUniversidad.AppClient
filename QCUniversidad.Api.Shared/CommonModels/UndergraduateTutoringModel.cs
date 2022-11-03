using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.CommonModels;

public record UndergraduateTutoringModel
{
    public int IntegrativeProjectDiplomants { get; set; }
    public int ThesisDiplomants { get; set; }
}
