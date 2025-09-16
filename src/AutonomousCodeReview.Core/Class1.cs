namespace AutonomousCodeReview.Core.Models;

public class CodeReviewRequest
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string RepositoryUrl { get; set; } = string.Empty;
    public string BranchName { get; set; } = "main";
    public string[] FilesToReview { get; set; } = Array.Empty<string>();
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public CodeReviewStatus Status { get; set; } = CodeReviewStatus.Pending;
}

public class CodeReviewResult
{
    public string RequestId { get; set; } = string.Empty;
    public List<ReviewComment> Comments { get; set; } = new();
    public int QualityScore { get; set; }
    public string Summary { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    public List<string> SuggestedImprovements { get; set; } = new();
}

public class ReviewComment
{
    public string FileName { get; set; } = string.Empty;
    public int LineNumber { get; set; }
    public string Message { get; set; } = string.Empty;
    public ReviewSeverity Severity { get; set; }
    public string AgentName { get; set; } = string.Empty;
}

public enum CodeReviewStatus
{
    Pending,
    InProgress,
    Completed,
    Failed
}

public enum ReviewSeverity
{
    Info,
    Warning,
    Error,
    Critical
}
