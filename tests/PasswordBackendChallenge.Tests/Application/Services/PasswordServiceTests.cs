namespace PasswordBackendChallenge.Tests.Application.Services;

public class PasswordServiceTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("aa", false)]
    [InlineData("ab", false)]
    [InlineData("AAAbbbCc", false)]
    [InlineData("AbTp9!foo", false)]
    [InlineData("ABCab 012!", false)]
    [InlineData("AABCbc012!", false)]
    [InlineData("AbTp9!foA", false)]
    [InlineData("AbTp9 fok", false)]
    [InlineData("AbTp9! fok", false)]
    [InlineData("ABCDabcd0123!@#$", true)]
    [InlineData("ABCab012!", true)]
    [InlineData("ABCabc012!", true)]
    [InlineData("AbTp9!fok", true)]
    public void ValidatePassword_WhenHasValidPassword_ShouldReturnSuccessResult(string value, bool expected)
    {
        // Arrange
        PasswordService sut = new(new Mock<ILogger<PasswordService>>().Object, new Mock<IPasswordMetric>().Object);
        PasswordSettings passwordSettings = Helper.CreatePasswordSettings;

        // Act
        Result actual = sut.ValidatePassword(passwordSettings.Complexity, value);

        // Assert
        if (expected)
        {
            Assert.IsType<SuccessResult>(actual);
        }
        else
        {
            Assert.IsType<ErrorResult>(actual);
        }
    }

    [Fact]
    public void ValidatePassword_WhenHasNullComplexities_ShouldReturnErrorResult()
    {
        // Arrange
        PasswordService sut = new(new Mock<ILogger<PasswordService>>().Object, new Mock<IPasswordMetric>().Object);

        // Act
        Result actual = sut.ValidatePassword(null, "AbTp9!fok");

        // Assert
        Assert.IsType<ErrorResult>(actual);
        ErrorResult errorResult = actual as ErrorResult;
        Assert.Equal("Nenhuma complexidade definida para validação", errorResult.Message);
    }

    [Fact]
    public void ValidatePassword_WhenHasEmptyComplexities_ShouldReturnErrorResult()
    {
        // Arrange
        PasswordService sut = new(new Mock<ILogger<PasswordService>>().Object, new Mock<IPasswordMetric>().Object);

        // Act
        Result actual = sut.ValidatePassword(new List<Complexity>(), "AbTp9!fok");

        // Assert
        Assert.IsType<ErrorResult>(actual);
        ErrorResult errorResult = actual as ErrorResult;
        Assert.Equal("Nenhuma complexidade definida para validação", errorResult.Message);
    }
}