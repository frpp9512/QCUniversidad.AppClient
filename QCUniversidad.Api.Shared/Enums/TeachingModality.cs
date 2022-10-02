﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Enums;

/// <summary>
/// The teaching modality that the students use for the career.
/// </summary>
public enum TeachingModality
{
    /// <summary>
    /// When the students are everyday in the campus reciveing the subjects.
    /// </summary>
    [Display(Name = "Curso regular diurno", Description = "Modalidad en la que los estudiantes acuden a las aulas diariamente.", Prompt = "Modalidad presencial")]
    Classroom,

    /// <summary>
    /// When the students recieve the subjects in specific dates in the months, the self study is the key.
    /// </summary>
    [Display(Name = "Curso por encuentros", Description = "Modalidad en la que los estudiantes acuden con menos frecuencia al aula y deben realizar mayor estudio individual.", Prompt = "Modalidad de curso por encuentros")]
    ByMeeting,

    /// <summary>
    /// When the students recieve the subjects via internet, and only make presence for the tests.
    /// </summary>
    [Display(Name = "Curso a distancia", Description = "Modalidad en la que los estudiantes no acuden a las aulas y deben de realizar el estudio individualmente.", Prompt = "Modalidad a distancia")]
    DistanceLearning
}