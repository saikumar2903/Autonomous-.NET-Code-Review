using System.Text.RegularExpressions;
using AutonomousCodeReview.Core.Interfaces;
using AutonomousCodeReview.Core.Models;

namespace AutonomousCodeReview.Core.Agents;

public class SecurityReviewAgent : ICodeReviewAgent
{
    public string Name => "Security Review Agent";
    public string Description => "Analyzes code for security vulnerabilities and best practices";

    private static readonly Dictionary<string, string> SecurityPatterns = new()
    {
        [@"\.ExecuteReader\(\s*[^)]*\s*\+"] = "Potential SQL injection vulnerability detected",
        [@"password\s*=\s*['""][^'""]*['""]"] = "Hardcoded password detected",
        [@"(?i)secret\s*=\s*['""][^'""]*['""]"] = "Hardcoded secret detected",
        [@"(?i)connectionstring\s*=\s*['""][^'""]*['""]"] = "Hardcoded connection string detected",
        [@"Process\.Start\("] = "Potential command injection vulnerability",
        [@"File\.ReadAllText\(\s*[^)]*\s*\+"] = "Potential path traversal vulnerability"
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
            foreach (var pattern in SecurityPatterns)
            {
                if (Regex.IsMatch(line, pattern.Key, RegexOptions.IgnoreCase))
                {
                    comments.Add(new ReviewComment
                    {
                        FileName = fileName,
                        LineNumber = i + 1,
                        Message = pattern.Value,
                        Severity = ReviewSeverity.Critical,
                        AgentName = Name
                    });
                }
            }
        }

        await Task.Delay(100); // Simulate AI processing time
        return comments;
    }
}