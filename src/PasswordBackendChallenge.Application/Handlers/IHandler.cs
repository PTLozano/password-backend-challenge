namespace PasswordBackendChallenge.Application.Handlers;

public interface IHandler
{
    IHandler SetNext(IHandler handler);

    Result? Handle(Complexity complexity, IDictionary<char, int> password);
}