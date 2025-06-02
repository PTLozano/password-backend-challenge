namespace PasswordBackendChallenge.Tests.Integration;

public class PasswordApiTests(WebApplicationFactory<PasswordController> factory)
    : IClassFixture<WebApplicationFactory<PasswordController>>
{
    [Theory]
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
    public async Task PostPassword(string value, bool expected)
    {
        // Arrange
        HttpClient client = factory.CreateClient();
        PasswordRequest request = new(value);

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/password/validate", request);

        // Assert
        if(expected)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            PasswordResponse responseBody = await response.Content.ReadFromJsonAsync<PasswordResponse>();
            Assert.NotNull(responseBody);
            Assert.True(responseBody.IsValid);
        }
        else
        {
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            PasswordResponse responseBody = await response.Content.ReadFromJsonAsync<PasswordResponse>();
            Assert.NotNull(responseBody);
            Assert.False(responseBody.IsValid);
            Assert.NotEmpty(responseBody.Message);
        }
    }
}