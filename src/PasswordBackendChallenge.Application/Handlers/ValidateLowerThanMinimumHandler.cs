namespace PasswordBackendChallenge.Application.Handlers;

public class ValidateLowerThanMinimumHandler : AbstractHandler
{
    public override Result Handle(Complexity complexity, IDictionary<char, int> password)
    {
        int validCharacterCount = password.Values.Sum();

        // Verifica se a quantidade de caracteres válidos é menor que o mínimo
        if (validCharacterCount < complexity.MinimumLength)
        {
            return new ErrorResult($"A senha deve conter pelo menos {complexity.MinimumLength} caracter(es) válido(s) para {complexity.Identifier}");
        }

        return base.Handle(complexity, password);
    }
}