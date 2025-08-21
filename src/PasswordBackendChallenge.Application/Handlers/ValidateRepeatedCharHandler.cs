namespace PasswordBackendChallenge.Application.Handlers;

public class ValidateRepeatedCharHandler : AbstractHandler
{
    public override Result Handle(Complexity complexity, IDictionary<char, int> password)
    {
        // Verifica se a quantidade de caracteres repetidos é maior que o máximo permitido
        if (password.Any(x => x.Value > complexity.MaximumRepeatCharCount))
        {
            return new ErrorResult($"Quantidade máxima de caracteres para {complexity.Identifier} iguais é de {complexity.MaximumRepeatCharCount}");
        }

        return base.Handle(complexity, password);
    }
}