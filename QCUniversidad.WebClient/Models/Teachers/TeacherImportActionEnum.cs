using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Teachers;

public enum TeacherImportAction
{
    [Display(Name = "Crear", Description = "Se va a crear un nuevo elemento con los datos extraídos del fichero.")]
    Create,

    [Display(Name = "Actualizar", Description = "Se va a actualizar un elemento existente con los datos extraídos del fichero.")]
    Update,

    [Display(Name = "No importar", Description = "No se va a importar el elemento debido a errores en los datos extraídos del fichero.")]
    NoImport,
}
