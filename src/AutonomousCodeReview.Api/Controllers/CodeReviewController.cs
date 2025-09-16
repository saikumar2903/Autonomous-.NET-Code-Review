using Microsoft.AspNetCore.Mvc;
using AutonomousCodeReview.Core.Interfaces;
using AutonomousCodeReview.Core.Models;

namespace AutonomousCodeReview.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CodeReviewController : ControllerBase
{
    private readonly IAgentOrchestrator _orchestrator;
    private readonly ILogger<CodeReviewController> _logger;

    public CodeReviewController(IAgentOrchestrator orchestrator, ILogger<CodeReviewController> logger)
    {
        _orchestrator = orchestrator;
        _logger = logger;
    }

    /// <summary>
    /// Submit a new code review request
    /// </summary>
    [HttpPost("submit")]
    public async Task<ActionResult<CodeReviewResult>> SubmitReviewRequest([FromBody] CodeReviewRequest request)
    {
        try
        {
            _logger.LogInformation("Processing code review request for repository: {Repository}", request.RepositoryUrl);
            
            var result = await _orchestrator.ProcessCodeReviewAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing code review request");
            return StatusCode(500, new { message = "Internal server error occurred" });
        }
    }

    /// <summary>
    /// Get the status of a code review request
    /// </summary>
    [HttpGet("status/{requestId}")]
    public async Task<ActionResult<CodeReviewStatus>> GetReviewStatus(string requestId)
    {
        var status = await _orchestrator.GetReviewStatusAsync(requestId);
        return Ok(new { requestId, status = status.ToString() });
    }

    /// <summary>
    /// Get all active code review requests
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<List<CodeReviewRequest>>> GetActiveReviews()
    {
        var activeReviews = await _orchestrator.GetActiveReviewsAsync();
        return Ok(activeReviews);
    }

    /// <summary>
    /// Submit a quick review for a code snippet
    /// </summary>
    [HttpPost("quick-review")]
    public async Task<ActionResult<CodeReviewResult>> QuickReview([FromBody] QuickReviewRequest request)
    {
        try
        {
            var reviewRequest = new CodeReviewRequest
            {
                Id = Guid.NewGuid().ToString(),
                RepositoryUrl = "snippet",
                FilesToReview = new[] { request.FileName }
            };

            var result = await _orchestrator.ProcessCodeReviewAsync(reviewRequest);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing quick review request");
            return StatusCode(500, new { message = "Internal server error occurred" });
        }
    }
}

public class QuickReviewRequest
{
    public string FileName { get; set; } = "snippet.cs";
    public string Code { get; set; } = string.Empty;
}