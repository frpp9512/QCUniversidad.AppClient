using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Enums
{
    /// <summary>
    /// The type of the teaching activities performed by a teacher in a specific subject in a period.
    /// </summary>
    public enum TeachingActivityType
    {
        /// <summary>
        /// Conferences face to face with students.
        /// </summary>
        [Display(Name = "Conferencia", Description = "Conferencia presencial con los estudiantes.", Prompt = "Conferencia")]
        Conference,

        /// <summary>
        /// Practical classes to put to test the learned 
        /// </summary>
        [Display(Name = "Clase práctica", Description = "Clase práctica poniendo a prueba lo explicado en la conferencia.", Prompt = "Clase práctica")]
        PracticalClass,

        /// <summary>
        /// CTP
        /// </summary>
        [Display(Name = "Clase teórico-práctica", Description = "Clases para la aplicación de conocimientos teóricos.", Prompt = "Clase teórico-práctica")]
        CTP,

        /// <summary>
        /// Laboratory classes to perform practical experiments.
        /// </summary>
        [Display(Name = "Laboratorio", Description = "Clases de laboratorio para poner en práctica experimentos en entornos controlados.", Prompt = "Laboratorio")]
        Laboratory,

        /// <summary>
        /// CLasses of a ByMeeting type of course.
        /// </summary>
        [Display(Name = "Clase encuentro", Description = "Clases a los estudiantes de la modalidad de curso por encuentro.", Prompt = "Clase encuentro")]
        MeetingClass,

        /// <summary>
        /// CLasses of a ByMeeting type of course.
        /// </summary>
        [Display(Name = "Curso de posgrado", Description = "Clases a los estudiantes de postgrado, maestría o doctorado.", Prompt = "Curso de posgrado")]
        PostgraduateClass,

        /// <summary>
        /// Other school activities
        /// </summary>
        [Display(Name = "Otra", Description = "Otros tipos de actividades docentes", Prompt = "Otra")]
        Other
    }
}