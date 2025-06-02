namespace PasswordBackendChallenge.Tests.Presentation.Controllers;

public class PasswordControllerTests
{
    [Fact]
    public async Task Index_WhenHasValidPassword_ShouldReturn200Ok()
    {
        // Arrange
        IPasswordMetric passwordMetricMock = new Mock<IPasswordMetric>().Object;
        PasswordService passwordServiceMock = new(new Mock<ILogger<PasswordService>>().Object, passwordMetricMock);
        TestOptionsMonitor<PasswordSettings> options = new TestOptionsMonitor<PasswordSettings>(Helper.CreatePasswordSettings);
        PasswordRequestValidator validator = new(options);
        PasswordController sut = new(
            new Mock<ILogger<PasswordController>>().Object,
            passwordMetricMock,
            passwordServiceMock,
            options,
            validator
        );

        // Act
        IActionResult actual = await sut.Validate(new("AbTp9!fok"));
        TestValidationResult<PasswordRequest> validate = await validator.TestValidateAsync(new PasswordRequest("AbTp9!fok"));

        // Assert
        validate.ShouldNotHaveAnyValidationErrors();
        Assert.IsType<OkObjectResult>(actual);
        OkObjectResult resp = actual as OkObjectResult;
        Assert.Equal((int)HttpStatusCode.OK, resp.StatusCode);
        PasswordResponse value = resp.Value as PasswordResponse;
        Assert.True(value.IsValid);
    }

    [Fact]
    public async Task Index_WhenHasInvalidPassword_ShouldReturn400BadRequest()
    {
        // Arrange
        IPasswordMetric passwordMetricMock = new Mock<IPasswordMetric>().Object;
        PasswordService passwordServiceMock = new(new Mock<ILogger<PasswordService>>().Object, passwordMetricMock);
        TestOptionsMonitor<PasswordSettings> options = new TestOptionsMonitor<PasswordSettings>(Helper.CreatePasswordSettings);
        PasswordRequestValidator validator = new(options);
        PasswordController sut = new(
            new Mock<ILogger<PasswordController>>().Object,
            passwordMetricMock,
            passwordServiceMock,
            options,
            validator
        );

        // Act
        IActionResult actual = await sut.Validate(new("AbTp9!foo"));
        TestValidationResult<PasswordRequest> validate = await validator.TestValidateAsync(new PasswordRequest("AbTp9!fok"));

        // Assert
        validate.ShouldNotHaveAnyValidationErrors();
        Assert.IsType<BadRequestObjectResult>(actual);
        BadRequestObjectResult resp = actual as BadRequestObjectResult;
        Assert.Equal((int)HttpStatusCode.BadRequest, resp.StatusCode);
        PasswordResponse value = resp.Value as PasswordResponse;
        Assert.False(value.IsValid);
    }

    [Fact]
    public async Task Index_WhenPasswordValidationFail_ShouldReturn400BadRequest()
    {
        // Arrange
        IPasswordMetric passwordMetricMock = new Mock<IPasswordMetric>().Object;
        PasswordService passwordServiceMock = new(new Mock<ILogger<PasswordService>>().Object, passwordMetricMock);
        TestOptionsMonitor<PasswordSettings> options = new TestOptionsMonitor<PasswordSettings>(Helper.CreatePasswordSettings);
        PasswordRequestValidator validator = new(options);
        PasswordController sut = new(
            new Mock<ILogger<PasswordController>>().Object,
            passwordMetricMock,
            passwordServiceMock,
            options,
            validator
        );

        // Act
        IActionResult actual = await sut.Validate(new("foo"));

        // Assert
        Assert.IsType<BadRequestObjectResult>(actual);
        BadRequestObjectResult resp = actual as BadRequestObjectResult;
        Assert.Equal((int)HttpStatusCode.BadRequest, resp.StatusCode);
        PasswordResponse value = resp.Value as PasswordResponse;
        Assert.False(value.IsValid);
    }
}