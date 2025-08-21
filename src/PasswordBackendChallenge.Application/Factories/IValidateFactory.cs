namespace PasswordBackendChallenge.Application.Factories;

public class ValidateComplexityFactory : IValidateFactory
{
    public AbstractHandler CreateHandler()
    {
        ValidateRepeatedCharHandler repeatedChar = new();
        ValidateLowerThanMinimumHandler lowerThanMinimum = new();
        ValidateBiggerThanMinimumHandler biggerThanMinimum = new();

        repeatedChar.SetNext(lowerThanMinimum).SetNext(biggerThanMinimum);

        return repeatedChar;
    }
}

public interface IValidateFactory
{
    public AbstractHandler CreateHandler();
}