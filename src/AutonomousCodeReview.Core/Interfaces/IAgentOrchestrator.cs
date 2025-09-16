using AutonomousCodeReview.Core.Models;

namespace AutonomousCodeReview.Core.Interfaces;

public interface IAgentOrchestrator
{
    Task<CodeReviewResult> ProcessCodeReviewAsync(CodeReviewRequest request);
    Task<CodeReviewStatus> GetReviewStatusAsync(string requestId);
    Task<List<CodeReviewRequest>> GetActiveReviewsAsync();
}