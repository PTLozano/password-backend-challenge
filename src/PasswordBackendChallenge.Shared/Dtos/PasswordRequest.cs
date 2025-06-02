namespace PasswordBackendChallenge.Shared.Dtos;

public record PasswordRequest(string Password);

public class PasswordRequestValidator : AbstractValidator<PasswordRequest>
{
    public PasswordRequestValidator(IOptionsMonitor<PasswordSettings> passwordSettings)
    {
        if (passwordSettings == null)
        {
            throw new ArgumentNullException(nameof(passwordSettings));
        }

        PasswordSettings currentSettings = passwordSettings.CurrentValue;
        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("A senha não pode estar vazia")
            .MinimumLength(currentSettings.MinimumPasswordLength)
                .WithMessage($"A senha precisa ter ao menos {currentSettings.MinimumPasswordLength} caracteres")
            .MaximumLength(currentSettings.MaximumPasswordLength)
                .WithMessage($"A senha deve ter no máximo {currentSettings.MaximumPasswordLength} caracteres");
    }
}