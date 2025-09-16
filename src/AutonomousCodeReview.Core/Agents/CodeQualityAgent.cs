using System.Text.RegularExpressions;
using AutonomousCodeReview.Core.Interfaces;
using AutonomousCodeReview.Core.Models;

namespace AutonomousCodeReview.Core.Agents;

public class CodeQualityAgent : ICodeReviewAgent
{
    public string Name => "Code Quality Agent";
    public string Description => "Analyzes code for quality issues, best practices, and maintainability";

    private static readonly Dictionary<string, (string Message, ReviewSeverity Severity)> QualityPatterns = new()
    {
        [@"(?i)todo|fixme|hack"] = ("TODO/FIXME comment found - consider addressing", ReviewSeverity.Warning),
        [@"catch\s*\(\s*Exception\s+\w+\s*\)\s*\{\s*\}"] = ("Empty catch block - should handle or log exceptions", ReviewSeverity.Error),
        [@"Console\.WriteLine"] = ("Console.WriteLine found - consider using proper logging", ReviewSeverity.Info),
        [@"public\s+class\s+\w+\s*\{[^}]{500,}"] = ("Large class detected - consider refactoring", ReviewSeverity.Warning),
        [@"public\s+\w+\s+\w+\([^)]{100,}\)"] = ("Method with many parameters - consider refactoring", ReviewSeverity.Warning),
        [@"if\s*\([^)]*\&\&[^)]*\&\&[^)]*\&\&"] = ("Complex conditional - consider extracting to method", ReviewSeverity.Info)
    };

    public bool CanReviewFile(string fileName)
    {
        return fileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<List<ReviewComment>> ReviewCodeAsync(string fileName, string fileContent)
    {
        var comments = new List<ReviewComment>();
        var lines = fileContent.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            foreach (var pattern in QualityPatterns)
            {
                if (Regex.IsMatch(line, pattern.Key, RegexOptions.IgnoreCase))
                {
                    comments.Add(new ReviewComment
                    {
                        FileName = fileName,
                        LineNumber = i + 1,
                        Message = pattern.Value.Message,
                        Severity = pattern.Value.Severity,
                        AgentName = Name
                    });
                }
            }
        }

        // Check for naming conventions
        CheckNamingConventions(fileName, fileContent, comments);

        await Task.Delay(150); // Simulate AI processing time
        return comments;
    }

    private void CheckNamingConventions(string fileName, string fileContent, List<ReviewComment> comments)
    {
        var lines = fileContent.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            
            // Check for private fields not starting with underscore
            if (Regex.IsMatch(line, @"private\s+\w+\s+[a-z]\w*\s*[=;]"))
            {
                comments.Add(new ReviewComment
                {
                    FileName = fileName,
                    LineNumber = i + 1,
                    Message = "Private field should start with underscore (_)",
                    Severity = ReviewSeverity.Info,
                    AgentName = Name
                });
            }
        }
    }
}