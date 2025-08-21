namespace PasswordBackendChallenge.Application.Handlers;

public class ValidateBiggerThanMinimumHandler : AbstractHandler
{
    public override Result Handle(Complexity complexity, IDictionary<char, int> password)
    {
        int validCharacterCount = password.Values.Sum();

        // Verifica se a quantidade de caracteres válidos é maior que o máximo
        if (validCharacterCount > complexity.MaximumLength)
        {
            return new ErrorResult($"A senha deve conter no máximo {complexity.MaximumLength} caracter(es) válido(s) para {complexity.Identifier}");
        }

        return base.Handle(complexity, password);
    }
}