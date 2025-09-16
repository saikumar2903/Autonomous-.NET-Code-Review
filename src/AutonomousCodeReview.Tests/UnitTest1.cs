using AutonomousCodeReview.Core.Agents;
using AutonomousCodeReview.Core.Models;

namespace AutonomousCodeReview.Tests;

public class SecurityReviewAgentTests
{
    private readonly SecurityReviewAgent _agent;

    public SecurityReviewAgentTests()
    {
        _agent = new SecurityReviewAgent();
    }

    [Fact]
    public void CanReviewFile_WithCSharpFile_ReturnsTrue()
    {
        // Arrange
        var fileName = "Program.cs";

        // Act
        var result = _agent.CanReviewFile(fileName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanReviewFile_WithNonCSharpFile_ReturnsFalse()
    {
        // Arrange
        var fileName = "README.md";

        // Act
        var result = _agent.CanReviewFile(fileName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithSqlInjectionVulnerability_ReturnsSecurityComment()
    {
        // Arrange
        var fileName = "DatabaseService.cs";
        var codeContent = "private string password = \"secret123\";";

        // Act
        var comments = await _agent.ReviewCodeAsync(fileName, codeContent);

        // Assert
        Assert.NotEmpty(comments);
        Assert.Contains(comments, c => c.Message.Contains("password"));
    }

    [Fact]
    public async Task ReviewCodeAsync_WithHardcodedPassword_ReturnsSecurityComment()
    {
        // Arrange
        var fileName = "Config.cs";
        var codeContent = @"
            private string password = ""secretPassword123"";
            var connectionString = ""Server=localhost;Password=admin123;"";
        ";

        // Act
        var comments = await _agent.ReviewCodeAsync(fileName, codeContent);

        // Assert
        Assert.NotEmpty(comments);
        Assert.Contains(comments, c => c.Severity == ReviewSeverity.Critical);
        Assert.Contains(comments, c => c.Message.Contains("password"));
    }

    [Fact]
    public async Task ReviewCodeAsync_WithCleanCode_ReturnsNoComments()
    {
        // Arrange
        var fileName = "CleanService.cs";
        var codeContent = @"
            public class CleanService
            {
                private readonly IConfiguration _config;
                
                public CleanService(IConfiguration config)
                {
                    _config = config;
                }
                
                public async Task<List<User>> GetUsersAsync(string userName)
                {
                    var connectionString = _config.GetConnectionString(""DefaultConnection"");
                    using var connection = new SqlConnection(connectionString);
                    
                    var query = ""SELECT * FROM Users WHERE Name = @userName"";
                    var users = await connection.QueryAsync<User>(query, new { userName });
                    
                    return users.ToList();
                }
            }
        ";

        // Act
        var comments = await _agent.ReviewCodeAsync(fileName, codeContent);

        // Assert
        Assert.Empty(comments);
    }
}