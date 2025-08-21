namespace PasswordBackendChallenge.Application.Handlers;

public abstract class AbstractHandler : IHandler
{
    private IHandler? _nextHandler;

    public IHandler SetNext(IHandler handler)
    {
        _nextHandler = handler;

        return handler;
    }

    public virtual Result Handle(Complexity complexity, IDictionary<char, int> password)
    {
        if (_nextHandler != null)
        {
            return _nextHandler.Handle(complexity, password);
        }

        int validCharacterCount = password.Values.Sum();

        return new SuccessResult(validCharacterCount);
    }
}