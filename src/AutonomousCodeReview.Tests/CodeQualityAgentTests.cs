using AutonomousCodeReview.Core.Agents;
using AutonomousCodeReview.Core.Models;

namespace AutonomousCodeReview.Tests;

public class CodeQualityAgentTests
{
    private readonly CodeQualityAgent _agent;

    public CodeQualityAgentTests()
    {
        _agent = new CodeQualityAgent();
    }

    [Fact]
    public void CanReviewFile_WithCSharpFile_ReturnsTrue()
    {
        // Arrange
        var fileName = "Service.cs";

        // Act
        var result = _agent.CanReviewFile(fileName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ReviewCodeAsync_WithTodoComment_ReturnsWarning()
    {
        // Arrange
        var fileName = "Service.cs";
        var codeContent = @"
            public void ProcessData()
            {
                // TODO: Implement data validation
                var data = GetData();
                ProcessDataInternal(data);
            }
        ";

        // Act
        var comments = await _agent.ReviewCodeAsync(fileName, codeContent);

        // Assert
        Assert.NotEmpty(comments);
        Assert.Contains(comments, c => c.Severity == ReviewSeverity.Warning && c.Message.Contains("TODO"));
    }

    [Fact]
    public async Task ReviewCodeAsync_WithConsoleWriteLine_ReturnsInfoComment()
    {
        // Arrange
        var fileName = "Service.cs";
        var codeContent = @"
            public void Debug()
            {
                Console.WriteLine(""Debug information"");
            }
        ";

        // Act
        var comments = await _agent.ReviewCodeAsync(fileName, codeContent);

        // Assert
        Assert.NotEmpty(comments);
        Assert.Contains(comments, c => c.Severity == ReviewSeverity.Info && c.Message.Contains("Console.WriteLine"));
    }

    [Fact]
    public async Task ReviewCodeAsync_WithEmptyCatchBlock_ReturnsError()
    {
        // Arrange
        var fileName = "Service.cs";
        var codeContent = "// TODO: implement this method";

        // Act
        var comments = await _agent.ReviewCodeAsync(fileName, codeContent);

        // Assert
        Assert.NotEmpty(comments);
        Assert.Contains(comments, c => c.Message.Contains("TODO"));
    }

    [Fact]
    public async Task ReviewCodeAsync_WithPoorNaming_ReturnsInfoComment()
    {
        // Arrange
        var fileName = "Service.cs";
        var codeContent = @"
            public class UserService
            {
                private string connectionString;
                private bool isActive;
            }
        ";

        // Act
        var comments = await _agent.ReviewCodeAsync(fileName, codeContent);

        // Assert
        Assert.NotEmpty(comments);
        Assert.Contains(comments, c => c.Message.Contains("underscore"));
    }
}