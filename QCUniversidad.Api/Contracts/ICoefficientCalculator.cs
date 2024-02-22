namespace QCUniversidad.Api.Contracts;

public interface ICoefficientCalculator<T>
{
    double CalculateValue(T model);
}