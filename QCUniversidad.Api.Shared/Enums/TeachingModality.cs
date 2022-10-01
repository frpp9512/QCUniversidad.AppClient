using System;
using System.Collections.Generic;
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
    Classroom,

    /// <summary>
    /// When the students recieve the subjects in specific dates in the months, the self study is the key.
    /// </summary>
    ByMeeting,

    /// <summary>
    /// When the students recieve the subjects via internet, and only make presence for the tests.
    /// </summary>
    DistanceLearning
}