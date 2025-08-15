namespace PasswordBackendChallenge.Application.Roles;

public interface IHandler
{
    IHandler SetNext(IHandler handler);
        
    Result Handle(Complexity complexity, IDictionary<char, int> password);
}

public abstract class AbstractHandler : IHandler
{
    private IHandler _nextHandler;

    public IHandler SetNext(IHandler handler)
    {
        this._nextHandler = handler;

        return handler;
    }
        
    public virtual Result Handle(Complexity complexity, IDictionary<char, int> password)
    {
        if (_nextHandler != null)
        {
            return _nextHandler.Handle(complexity, password);
        }

        int validCharacterCount = password.Values.Sum();

        return new SuccessResult(validCharacterCount);;
    }
}

public class ValidateRepeatedCharHandler : AbstractHandler
{
    public override Result Handle(Complexity complexity, IDictionary<char, int> password)
    {
        // Verifica se a quantidade de caracteres repetidos é maior que o máximo permitido
        if (password.Any(x => x.Value > complexity.MaximumRepeatCharCount))
        {
            // metric.AddErrorCount("maximum_repeat_char_count_exceeded");

            return new ErrorResult($"Quantidade máxima de caracteres para {complexity.Identifier} iguais é de {complexity.MaximumRepeatCharCount}");
        }

        return base.Handle(complexity, password);
    }
}
public class ValidateLowerThanMinimumHandler : AbstractHandler
{
    public override Result Handle(Complexity complexity, IDictionary<char, int> password)
    {
        int validCharacterCount = password.Values.Sum();

        // Verifica se a quantidade de caracteres válidos é menor que o mínimo
        if (validCharacterCount < complexity.MinimumLength)
        {
            // metric.AddErrorCount("minimum_length_not_met");

            return new ErrorResult($"A senha deve conter pelo menos {complexity.MinimumLength} caracter(es) válido(s) para {complexity.Identifier}");
        }

        return base.Handle(complexity, password);
    }
}
public class ValidateBiggerThanMinimumHandler : AbstractHandler
{
    public override Result Handle(Complexity complexity, IDictionary<char, int> password)
    {
        int validCharacterCount = password.Values.Sum();

        // Verifica se a quantidade de caracteres válidos é maior que o máximo
        if (validCharacterCount > complexity.MaximumLength)
        {
            // metric.AddErrorCount("maximum_length_exceeded");

            return new ErrorResult($"A senha deve conter no máximo {complexity.MaximumLength} caracter(es) válido(s) para {complexity.Identifier}");
        }

        return base.Handle(complexity, password);
    }
}