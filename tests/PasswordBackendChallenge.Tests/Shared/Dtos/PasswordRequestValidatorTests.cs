namespace PasswordBackendChallenge.Tests.Shared.Dtos;

public class PasswordRequestValidatorTests
{
    [Fact]
    public async Task PasswordValidation_WhenPasswordIsNull_ShouldReturnError()
    {
        // Arrange
        TestOptionsMonitor<PasswordSettings> options = new TestOptionsMonitor<PasswordSettings>(Helper.CreatePasswordSettings);
        PasswordRequestValidator validator = new(options);

        // Act
        TestValidationResult<PasswordRequest> validate = await validator.TestValidateAsync(new PasswordRequest(null));

        // Assert
        validate.ShouldHaveValidationErrors();
    }

    [Fact]
    public async Task PasswordValidation_WhenPasswordIsEmpty_ShouldReturnError()
    {
        // Arrange
        TestOptionsMonitor<PasswordSettings> options = new TestOptionsMonitor<PasswordSettings>(Helper.CreatePasswordSettings);
        PasswordRequestValidator validator = new(options);

        // Act
        TestValidationResult<PasswordRequest> validate = await validator.TestValidateAsync(new PasswordRequest("     "));

        // Assert
        validate.ShouldHaveValidationErrors();
    }

    [Fact]
    public async Task PasswordValidation_WhenPasswordHasLessCharThanMinimum_ShouldReturnError()
    {
        // Arrange
        TestOptionsMonitor<PasswordSettings> options = new TestOptionsMonitor<PasswordSettings>(Helper.CreatePasswordSettings);
        PasswordRequestValidator validator = new(options);

        // Act
        TestValidationResult<PasswordRequest> validate = await validator.TestValidateAsync(new PasswordRequest("Test!"));

        // Assert
        validate.ShouldHaveValidationErrors();
    }

    [Fact]
    public async Task PasswordValidation_WhenPasswordHasMoreCharThanMaximum_ShouldReturnError()
    {
        // Arrange
        TestOptionsMonitor<PasswordSettings> options = new TestOptionsMonitor<PasswordSettings>(Helper.CreatePasswordSettings);
        PasswordRequestValidator validator = new(options);

        // Act
        TestValidationResult<PasswordRequest> validate = await validator.TestValidateAsync(new PasswordRequest("Test to validate more char than the maximum value!"));

        // Assert
        validate.ShouldHaveValidationErrors();
    }
}