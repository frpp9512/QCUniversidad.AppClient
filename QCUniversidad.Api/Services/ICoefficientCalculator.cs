namespace QCUniversidad.Api.Services;

public interface ICoefficientCalculator<T>
{
    double CalculateValue(T model);
}