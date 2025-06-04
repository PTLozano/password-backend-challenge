namespace PasswordBackendChallenge.Application.Services;

public sealed class PasswordService(ILogger<PasswordService> logger, IPasswordMetric metric) : IPasswordService
{
    public Result ValidatePassword(IReadOnlyCollection<Complexity> complexities, string password)
    {
        Result result = CheckPasswordComplexities(complexities, password);

        if (!result.IsValid)
        {
            logger.LogError("Senha inválida: {Message}", result.Message);

            return new ErrorResult(result.Message);
        }

        if (result.ValidCharacterCount == password.Length)
        {
            logger.LogTrace("Senha válida");

            return new SuccessResult(result.ValidCharacterCount);
        }

        logger.LogError("Senha inválida, contém caracteres não permitidos");

        return new ErrorResult("Senha inválida, contém caracteres não permitidos");
    }

    private Result CheckPasswordComplexities(IReadOnlyCollection<Complexity> complexities, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            metric.AddErrorCount("empty_password");

            logger.LogError("A senha não pode ser vazia");

            return new ErrorResult("A senha não pode ser vazia");
        }

        if (complexities == null || complexities.Count == 0)
        {
            metric.AddErrorCount("complexities_not_defined");

            logger.LogError("Nenhuma complexidade definida para validação");

            return new ErrorResult("Nenhuma complexidade definida para validação");
        }

        int validCharacterCount = 0;
        foreach (Complexity complexity in complexities)
        {
            if (!complexity.Enabled)
            {
                logger.LogTrace("Complexidade '{Identifier}' está desabilitada, pulando validação", complexity.Identifier);

                continue;
            }

            Result result = ValidateComplexity(complexity, password);

            if (!result.IsValid)
            {
                logger.LogError("Validação falhou para a complexidade '{Identifier}': {Message}", complexity.Identifier, result.Message);

                return new ErrorResult(result.Message);
            }

            validCharacterCount += result.ValidCharacterCount;
        }

        return new SuccessResult(validCharacterCount);
    }

    private Result ValidateComplexity(Complexity complexity, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return new ErrorResult("A senha não pode ser vazia");
        }

        var dictionary = new Dictionary<char, int>();
        char[] characters = complexity.Characters.ToCharArray();
        foreach (char c in password)
        {
            if (!characters.Contains(c))
            {
                continue;
            }

            // Verifica se o caracter já foi adicionado ao dicionário
            if (!dictionary.TryAdd(c, 1))
            {
                // Se já existe, incrementa o contador
                dictionary[c]++;
            }
        }

        // Verifica se a quantidade de caracteres repetidos é maior que o máximo permitido
        if (dictionary.Any(x => x.Value > complexity.MaximumRepeatCharCount))
        {
            metric.AddErrorCount("maximum_repeat_char_count_exceeded");

            return new ErrorResult($"Quantidade máxima de caracteres para {complexity.Identifier} iguais é de {complexity.MaximumRepeatCharCount}");
        }

        int validCharacterCount = dictionary.Values.Sum();

        // Verifica se a quantidade de caracteres válidos é menor que o mínimo
        if (validCharacterCount < complexity.MinimumLength)
        {
            metric.AddErrorCount("minimum_length_not_met");

            return new ErrorResult($"A senha deve conter pelo menos {complexity.MinimumLength} caracter(es) válido(s) para {complexity.Identifier}");
        }

        // Verifica se a quantidade de caracteres válidos é maior que o máximo
        if (validCharacterCount > complexity.MaximumLength)
        {
            metric.AddErrorCount("maximum_length_exceeded");

            return new ErrorResult($"A senha deve conter no máximo {complexity.MaximumLength} caracter(es) válido(s) para {complexity.Identifier}");
        }

        // Retorna a soma dos valores do dicionário, que representa a quantidade de caracteres válidos
        return new SuccessResult(validCharacterCount);
    }
}

public interface IPasswordService
{
    Result ValidatePassword(IReadOnlyCollection<Complexity> complexities, string password);
}