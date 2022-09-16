using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QCUniversidad.WebClient.Models
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre del rol", Description = "Nombre único que identifica al rol.")]
        public string Name { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción", Description = "Descripción de las acciones que puede ejecutar quien desepmeñe el rol.")]
        public string Description { get; set; }
    }
}
