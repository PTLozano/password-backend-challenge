namespace PasswordBackendChallenge.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PasswordController(
    ILogger<PasswordController> logger,
    IPasswordMetric metric,
    IPasswordService passwordService,
    IOptionsMonitor<PasswordSettings> passwordSettings,
    IValidator<PasswordRequest> validator) : Controller
{
    [ApiKey]
    // [EnableRateLimiting("password-validate")]
    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] PasswordRequest data)
    {
        using (metric.AddProcessDuration(nameof(PasswordController)))
        {
            ValidationResult validate = await validator.ValidateAsync(data);
            if (!validate.IsValid)
            {
                metric.AddErrorCount("validation_error");
                logger.LogWarning("Senha informada é inválida: {Errors}", validate.Errors);

                return BadRequest(new PasswordResponse(false) { Message = string.Join("; ", validate.Errors.Select(x => x.ErrorMessage).Distinct()) });
            }

            PasswordSettings currentSettings = passwordSettings.CurrentValue;

            Result result = passwordService.ValidatePassword(currentSettings.Complexity, data.Password);

            logger.LogTrace("Validação da senha informada: {Result}", result.IsValid ? "Válida" : "Inválida");

            if (result.IsValid)
            {
                metric.AddResult("valid_password");

                return Ok(new PasswordResponse(true));
            }

            metric.AddErrorCount("invalid_password");

            return BadRequest(new PasswordResponse(false) { Message = result.Message });
        }
    }
}