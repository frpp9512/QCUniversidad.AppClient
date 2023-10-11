using System.Linq.Expressions;
using QCUniversidad.Api.Contracts;

namespace QCUniversidad.Api.Services;

public class CoefficientCalculator<T> : ICoefficientCalculator<T>
{
    private readonly Func<T, double> _coeffFunction;
    private readonly Expression<Func<T, double>> _memberExpression;
    private readonly Func<double, double>? _afterCalcFunction = null;

    public CoefficientCalculator(double coeff, Expression<Func<T, double>> valueMemberExpression)
    {
        _coeffFunction = _ => coeff;
        _memberExpression = valueMemberExpression;
    }

    public CoefficientCalculator(double coeff, Expression<Func<T, double>> memberExpression, Func<double, double> afterCalcFunc)
    {
        _coeffFunction = _ => coeff;
        _memberExpression = memberExpression;
        _afterCalcFunction = afterCalcFunc;
    }

    public CoefficientCalculator(Func<T, double> coeffFunction, Expression<Func<T, double>> memberExpression)
    {
        _coeffFunction = coeffFunction;
        _memberExpression = memberExpression;
    }

    public CoefficientCalculator(Func<T, double> coeffFunction, Expression<Func<T, double>> memberExpression, Func<double, double> afterCalcFunc)
    {
        _coeffFunction = coeffFunction;
        _memberExpression = memberExpression;
        _afterCalcFunction = afterCalcFunc;
    }

    public double CalculateValue(T model) => _afterCalcFunction is null ? CoeffCalculation(model) : _afterCalcFunction(CoeffCalculation(model));

    private double CoeffCalculation(T model) => _coeffFunction(model) * _memberExpression.Compile().Invoke(model);
}