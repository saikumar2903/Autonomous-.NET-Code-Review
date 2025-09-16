using AutonomousCodeReview.Core.Models;

namespace AutonomousCodeReview.Core.Interfaces;

public interface ICodeReviewAgent
{
    string Name { get; }
    string Description { get; }
    Task<List<ReviewComment>> ReviewCodeAsync(string fileName, string fileContent);
    bool CanReviewFile(string fileName);
}