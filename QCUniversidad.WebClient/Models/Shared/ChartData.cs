using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;

namespace QCUniversidad.WebClient.Models.Shared;

public static class ModelListCharter
{
    private static string GetTypeName<T>()
    {
        var typeName = typeof(T).Name;
        var classDisplayAttr = typeof(T).GetCustomAttribute(typeof(DisplayAttribute));
        if (classDisplayAttr is not null)
        {
            var cDisplayAttr = classDisplayAttr as DisplayAttribute;
            if (!string.IsNullOrEmpty(cDisplayAttr.Name))
            {
                typeName = cDisplayAttr.Name;
            }
        }
        return typeName;
    }

    private static string? GetExpressionMemberName(LambdaExpression expression)
    {
        if (expression.Body.NodeType == ExpressionType.MemberAccess)
        {
            var member = (MemberExpression)expression.Body;
            var name = member.Member.Name;
            var propDisplayAttr = member.Member.GetCustomAttribute(typeof(DisplayAttribute));
            if (propDisplayAttr is not null)
            {
                var pDisplayAttr = propDisplayAttr as DisplayAttribute;
                if (!string.IsNullOrEmpty(pDisplayAttr.Name))
                {
                    name = pDisplayAttr.Name;
                }
            }
            return name;
        }
        return null;
    }

    public readonly static Color[] BackColors = new[]
        {
            Color.FromArgb(180, 81, 43, 212),
            Color.FromArgb(180, Color.FromKnownColor(KnownColor.MediumVioletRed)),
            Color.FromArgb(180, Color.FromKnownColor(KnownColor.Gold)),
            Color.FromArgb(180, Color.FromKnownColor(KnownColor.LightSeaGreen)),
            Color.FromArgb(180, Color.FromKnownColor(KnownColor.Tomato)),
            Color.FromArgb(180, Color.FromKnownColor(KnownColor.PaleTurquoise)),
            Color.FromArgb(180, Color.FromKnownColor(KnownColor.DodgerBlue)),
            Color.FromArgb(180, Color.FromKnownColor(KnownColor.Maroon)),
            Color.FromArgb(180, Color.FromKnownColor(KnownColor.Thistle)),
        };

    public readonly static Color[] BorderColors = new[]
        {
            Color.FromArgb(255, 81, 43, 212),
            Color.FromArgb(255, Color.FromKnownColor(KnownColor.MediumVioletRed)),
            Color.FromArgb(255, Color.FromKnownColor(KnownColor.Gold)),
            Color.FromArgb(255, Color.FromKnownColor(KnownColor.LightSeaGreen)),
            Color.FromArgb(255, Color.FromKnownColor(KnownColor.Tomato)),
            Color.FromArgb(255, Color.FromKnownColor(KnownColor.PaleTurquoise)),
            Color.FromArgb(255, Color.FromKnownColor(KnownColor.DodgerBlue)),
            Color.FromArgb(255, Color.FromKnownColor(KnownColor.Turquoise)),
            Color.FromArgb(255, Color.FromKnownColor(KnownColor.Thistle)),
        };

    public static ChartModel GetChartModel<T>(ChartType chartType,
                                              IList<T> modelList,
                                              Expression<Func<T, double>> valueSelector,
                                              Expression<Func<T, string>> labelSelector,
                                              ChartLegendPosition legendPosition = ChartLegendPosition.Top,
                                              string? title = null,
                                              string? subtitle = null,
                                              bool showXScale = true,
                                              bool showYScale = true,
                                              string? xScaleTitle = null,
                                              string? yScaleTitle = null,
                                              bool showXGrid = true,
                                              bool showYGrid = true,
                                              bool alternateColors = true,
                                              int borderWidth = 1,
                                              int borderRadius = 0,
                                              ChartPointStyle pointStyle = ChartPointStyle.Circle,
                                              int pointRadius = 5,
                                              int pointHoverRadius = 7,
                                              bool fill = false,
                                              double tension = 0,
                                              Color? borderColor = null,
                                              Color? backgroundColor = null)
    {
        var typeName = GetTypeName<T>();
        var label = GetExpressionMemberName(valueSelector) ?? typeName;

        var currentColor = 0;
        var nextColor = () =>
        {
            currentColor++;
            if (currentColor >= BackColors.Length)
            {
                currentColor = 0;
            }
            return (BackColors[currentColor], BorderColors[currentColor]);
        };

        var data = new ChartData
        {
            Labels = modelList.Select(m => labelSelector.Compile().Invoke(m)).ToArray(),
            DataSets = new[] {
                new ChartDataEntry
                {
                    Label = label,
                    BorderRadius = borderRadius,
                    BorderWidth = borderWidth,
                    PointStyle = pointStyle,
                    PointRadius = pointRadius,
                    PointHoverRadius = pointHoverRadius,
                    Fill = fill,
                    Tension = tension,
                    DataValues = modelList.Select(m =>
                    {
                        var (backcolor, bordercolor) = nextColor();
                        return new ChartDataValue
                        {
                            Value = valueSelector.Compile().Invoke(m),
                            BackgroundColor = backgroundColor ?? (alternateColors ? backcolor : BackColors.First()),
                            BorderColor =  borderColor ?? (alternateColors ? bordercolor : BorderColors.First())
                        };
                    })
                    .ToArray()
                }
            }
        };

        var chartModel = new ChartModel
        {
            Type = chartType,
            LegendPosition = legendPosition,
            Title = title ?? $"Gráfico de {typeName}",
            Subtitle = subtitle ?? "",
            Data = data,
            ElementId = $"{typeName}-chart",
            ShowSubtitle = subtitle is not null,
            ShowXScale = showXScale,
            ShowYScale = showYScale,
            XScaleTitle = xScaleTitle,
            YScaleTitle = yScaleTitle,
            ShowXGrid = showXGrid,
            ShowYGrid = showYGrid
        };

        return chartModel;
    }

    public static ChartModel GetChartModel<T, U>(ChartType chartType,
                                                 IList<T> modelList1,
                                                 Expression<Func<T, double>> valueSelector1,
                                                 IList<U> modelList2,
                                                 Expression<Func<U, double>> valueSelector2,
                                                 string[] labels,
                                                 string title = null,
                                                 string subtitle = null,
                                                 bool showXScale = true,
                                                 bool showYScale = true,
                                                 string? xScaleTitle = null,
                                                 string? yScaleTitle = null,
                                                 bool showXGrid = true,
                                                 bool showYGrid = true,
                                                 ChartLegendPosition? legendPosition = null,
                                                 bool alternateColors = true,
                                                 int borderWidth = 1,
                                                 int borderRadius = 0,
                                                 ChartPointStyle pointStyle = ChartPointStyle.Circle,
                                                 int pointRadius = 5,
                                                 int pointHoverRadius = 7,
                                                 bool fill = false,
                                                 double tension = 0)
    {
        var typeName1 = GetTypeName<T>();
        var typeName2 = GetTypeName<U>();
        var label1 = GetExpressionMemberName(valueSelector1) ?? typeName1;
        var label2 = GetExpressionMemberName(valueSelector2) ?? typeName2;

        var currentColor = 0;
        var nextColor = () =>
        {
            currentColor++;
            if (currentColor >= BackColors.Length)
            {
                currentColor = 0;
            }
            return (BackColors[currentColor], BorderColors[currentColor]);
        };

        var data = new ChartData
        {
            Labels = labels,
            DataSets = new[] {
                new ChartDataEntry
                {
                    Label = label1,
                    BorderWidth = borderWidth,
                    BorderRadius = borderRadius,
                    PointStyle = pointStyle,
                    PointHoverRadius = pointHoverRadius,
                    PointRadius = pointRadius,
                    Fill = fill,
                    Tension = tension,
                    DataValues = modelList1.Select(m =>
                    {
                        var (backcolor, borderColor) = nextColor();
                        return new ChartDataValue
                        {
                            Value = valueSelector1.Compile().Invoke(m),
                            BackgroundColor = alternateColors ? backcolor : BackColors.First(),
                            BorderColor = alternateColors ? borderColor : BorderColors.First()
                        };
                    })
                    .ToArray()
                },
                new ChartDataEntry
                {
                    Label = label2,
                    BorderWidth = borderWidth,
                    BorderRadius = borderRadius,
                    PointStyle = pointStyle,
                    PointHoverRadius = pointHoverRadius,
                    PointRadius = pointRadius,
                    Fill = fill,
                    DataValues = modelList2.Select(m =>
                    {
                        var (backcolor, borderColor) = nextColor();
                        return new ChartDataValue
                        {
                            Value = valueSelector2.Compile().Invoke(m),
                            BackgroundColor = alternateColors ? backcolor : BackColors.Skip(1).First(),
                            BorderColor = alternateColors ? borderColor : BorderColors.Skip(1).First()
                        };
                    })
                    .ToArray()
                }
            }
        };

        var chartModel = new ChartModel
        {
            Type = chartType,
            LegendPosition = legendPosition ?? ChartLegendPosition.Top,
            Title = title ?? $"Gráfico de {typeName1} y {typeName2}",
            Subtitle = subtitle ?? "",
            Data = data,
            ElementId = $"{typeName1}-{typeName2}-chart",
            ShowSubtitle = subtitle is not null,
            ShowXScale = showXScale,
            ShowYScale = showYScale,
            XScaleTitle = xScaleTitle,
            YScaleTitle = yScaleTitle,
            ShowXGrid = showXGrid,
            ShowYGrid = showYGrid
        };

        return chartModel;
    }
}

public record ChartModel
{
    public ChartType Type { get; set; }
    public string Title { get; set; }
    public string? Subtitle { get; set; }
    public ChartLegendPosition LegendPosition { get; set; }
    public ChartData Data { get; set; }
    public bool Responsive { get; set; } = true;
    public bool ShowTitle { get; set; } = true;
    public bool ShowSubtitle { get; set; }
    public bool ShowXScale { get; set; } = true;
    public string? XScaleTitle { get; set; } = null;
    public bool ShowYScale { get; set; } = true;
    public string? YScaleTitle { get; set; } = null;
    public bool ShowXGrid { get; set; } = true;
    public bool ShowYGrid { get; set; } = true;
    public bool Stacked { get; set; } = false;

    public string ElementId { get; set; }

    public object GetDataObject()
    {
        var dataObject = new
        {
            labels = Data.Labels,
            datasets = Data.DataSets.Select(ds => new
            {
                label = ds.Label,
                data = ds.Data,
                borderColor = ds.BorderColor,
                backgroundColor = ds.BackgroudColor,
                borderWidth = ds.BorderWidth,
                borderRadius = ds.BorderRadius,
                pointStyle = ds.PointStyle switch
                {
                    ChartPointStyle.Circle => "circle",
                    ChartPointStyle.Rect => "rect",
                    ChartPointStyle.RectRot => "rectRot",
                    ChartPointStyle.Start => "star",
                    ChartPointStyle.RectRounded => "rectRounded",
                    ChartPointStyle.Dash => "dash",
                    ChartPointStyle.Line => "line",
                    ChartPointStyle.Triangle => "triangle",
                    ChartPointStyle.Cross => "cross",
                    ChartPointStyle.CrossRot => "crossRot",
                    _ => "circle"
                },
                pointRadius = ds.PointRadius,
                pointHoverRadius = ds.PointHoverRadius,
                fill = ds.Fill,
                tension = ds.Tension
            })
        };
        return dataObject;
    }

    public string GetDataJson() => JsonConvert.SerializeObject(GetDataObject());

    public object GetConfigObject()
    {
        var configObject = new
        {
            type = Type switch
            {
                ChartType.Pie => "pie",
                ChartType.Line => "line",
                ChartType.Bar => "bar",
                ChartType.Doughnut => "doughnut",
                ChartType.PolarArea => "polarArea",
                ChartType.Radar => "radar",
                ChartType.Scatter => "scatter",
                _ => "bar"
            },
            data = GetDataObject(),
            options = new
            {
                responsive = Responsive,
                plugins = new
                {
                    legend = new
                    {
                        position = LegendPosition switch
                        {
                            ChartLegendPosition.Left => "left",
                            ChartLegendPosition.Right => "right",
                            ChartLegendPosition.Top => "top",
                            ChartLegendPosition.Bottom => "bottom",
                            _ => "top"
                        }
                    },
                    title = new
                    {
                        display = ShowTitle,
                        text = Title
                    },
                    subtitle = new
                    {
                        display = ShowSubtitle,
                        text = Subtitle
                    }
                },
                scales = new
                {
                    x = new
                    {
                        stacked = Stacked,
                        display = ShowXScale,
                        title = new
                        {
                            display = XScaleTitle is not null,
                            text = XScaleTitle ?? ""
                        },
                        grid = new
                        {
                            display = ShowXGrid
                        }
                    },
                    y = new
                    {
                        stacked = Stacked,
                        display = ShowYScale,
                        title = new
                        {
                            display = YScaleTitle is not null,
                            text = YScaleTitle ?? ""
                        },
                        grid = new
                        {
                            display = ShowYGrid
                        }
                    }
                },
                onclick = "handleClick",
            }
        };
        return configObject;
    }

    public string GetConfigJson() => JsonConvert.SerializeObject(GetConfigObject());

    public string GetJson()
    {
        var configObj = GetConfigObject();
        return JsonConvert.SerializeObject(new
        {
            config = configObj,
            elementId = ElementId
        });
    }
}

public record ChartData
{
    public string[] Labels { get; set; }
    public ChartDataEntry[] DataSets { get; set; }
}

public record ChartDataEntry
{
    public string Label { get; set; }
    public ChartDataValue[] DataValues { get; set; }
    public double[] Data => DataValues.Select(dv => dv.Value).ToArray();
    public string[] BorderColor => DataValues.Select(dv => $"rgba({dv.BorderColor.R}, {dv.BorderColor.G}, {dv.BorderColor.B}, {Math.Round((double)dv.BorderColor.A / 255, 1)})").ToArray();
    public string[] BackgroudColor => DataValues.Select(dv => $"rgba({dv.BackgroundColor.R},{dv.BackgroundColor.G},{dv.BackgroundColor.B},{Math.Round((double)dv.BackgroundColor.A / 255, 1)})").ToArray();
    public ChartPointStyle PointStyle { get; set; } = ChartPointStyle.Circle;
    public int PointRadius { get; set; } = 5;
    public int PointHoverRadius { get; set; } = 7;
    public int BorderWidth { get; set; } = 1;
    public int BorderRadius { get; set; }
    public double Tension { get; set; }
    public bool Fill { get; set; }
}

public record ChartDataValue
{
    public double Value { get; set; }
    public Color BackgroundColor { get; set; }
    public Color BorderColor { get; set; }
}

public enum ChartType
{
    Bar,
    Pie,
    Doughnut,
    Line,
    PolarArea,
    Radar,
    Scatter
}

public enum ChartLegendPosition
{
    Top,
    Right,
    Bottom,
    Left
}

public enum ChartPointStyle
{
    Circle,
    Cross,
    CrossRot,
    Dash,
    Line,
    Rect,
    RectRounded,
    RectRot,
    Start,
    Triangle
}