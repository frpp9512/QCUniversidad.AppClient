﻿using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Subjects;

public class EditSubjectModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Debe de definir el nombre de la asignatura.")]
    [Display(Name = "Nombre de la asignatura", Prompt = "Nombre de la asignatura", Description = "El nombre de la asignatura.")]
    public required string Name { get; set; }

    [Display(Name = "Descripción", Prompt = "Descripción de la asignatura", Description = "La descripción de la asignatura.")]
    public string? Description { get; set; }

    public Guid DisciplineId { get; set; }

    public required string DisciplineName { get; set; }
}
