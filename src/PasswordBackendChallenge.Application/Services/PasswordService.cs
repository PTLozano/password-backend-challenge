namespace PasswordBackendChallenge.Application.Services;

public sealed class PasswordService(ILogger<PasswordService> logger, IPasswordMetric metric) : IPasswordService
{
    public Result Validate(IReadOnlyCollection<Complexity> complexities, string password)
    {
        Result result = CheckComplexities(complexities, password);

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

    private Result CheckComplexities(IReadOnlyCollection<Complexity> complexities, string password)
    {
        Result hasComplexity = HasComplexity(complexities);
        if (!hasComplexity.IsValid)
        {
            return hasComplexity;
        }

        int validCharacterCount = 0;
        foreach (Complexity complexity in complexities)
        {
            if (!complexity.Enabled)
            {
                logger.LogWarning("Complexidade '{Identifier}' está desabilitada, pulando validação", complexity.Identifier);

                continue;
            }

            Result result = ValidateComplexity(complexity, password);

            if (!result.IsValid)
            {
                logger.LogError("Validação falhou para a complexidade '{Identifier}': {Message}", complexity.Identifier,
                                result.Message);

                return new ErrorResult(result.Message);
            }

            validCharacterCount += result.ValidCharacterCount;
        }

        return new SuccessResult(validCharacterCount);
    }

    private Result HasComplexity(IReadOnlyCollection<Complexity> complexities)
    {
        if (complexities.Count == 0)
        {
            metric.AddErrorCount("complexities_not_defined");

            logger.LogError("Nenhuma complexidade definida para validação");

            return new ErrorResult("Nenhuma complexidade definida para validação");
        }

        return new SuccessResult(-1);
    }

    private static Result ValidateComplexity(Complexity complexity, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return new ErrorResult("A senha não pode ser vazia");
        }

        Dictionary<char, int> dictionary = ProcessChars(complexity, password);

        AbstractHandler handlerChain = new ValidateComplexityFactory().CreateHandler();

        Result result = handlerChain.Handle(complexity, dictionary);

        return result.IsValid ?
            // Retorna a soma dos valores do dicionário, que representa a quantidade de caracteres válidos
            new SuccessResult(result.ValidCharacterCount) :
            result;
    }

    private static Dictionary<char, int> ProcessChars(Complexity complexity, string password)
    {
        Dictionary<char, int> dictionary = new Dictionary<char, int>();
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

        return dictionary;
    }
}

public interface IPasswordService
{
    Result Validate(IReadOnlyCollection<Complexity> complexities, string password);
}