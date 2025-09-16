using AutonomousCodeReview.Core.Interfaces;
using AutonomousCodeReview.Core.Models;

namespace AutonomousCodeReview.Core.Services;

public class AgentOrchestratorService : IAgentOrchestrator
{
    private readonly List<ICodeReviewAgent> _agents;
    private readonly Dictionary<string, CodeReviewRequest> _activeReviews;
    private readonly Dictionary<string, CodeReviewResult> _reviewResults;

    public AgentOrchestratorService(IEnumerable<ICodeReviewAgent> agents)
    {
        _agents = agents.ToList();
        _activeReviews = new Dictionary<string, CodeReviewRequest>();
        _reviewResults = new Dictionary<string, CodeReviewResult>();
    }

    public async Task<CodeReviewResult> ProcessCodeReviewAsync(CodeReviewRequest request)
    {
        _activeReviews[request.Id] = request;
        request.Status = CodeReviewStatus.InProgress;

        var result = new CodeReviewResult
        {
            RequestId = request.Id,
            Comments = new List<ReviewComment>(),
            Summary = "Code review completed by AI agents",
            SuggestedImprovements = new List<string>()
        };

        try
        {
            // Simulate fetching code from repository
            var codeFiles = await FetchCodeFilesAsync(request);

            // Process each file with applicable agents
            foreach (var (fileName, content) in codeFiles)
            {
                var applicableAgents = _agents.Where(agent => agent.CanReviewFile(fileName)).ToList();
                
                foreach (var agent in applicableAgents)
                {
                    var agentComments = await agent.ReviewCodeAsync(fileName, content);
                    result.Comments.AddRange(agentComments);
                }
            }

            // Calculate quality score based on issues found
            result.QualityScore = CalculateQualityScore(result.Comments);
            
            // Generate suggestions
            result.SuggestedImprovements = GenerateSuggestions(result.Comments);

            request.Status = CodeReviewStatus.Completed;
            _reviewResults[request.Id] = result;
        }
        catch (Exception ex)
        {
            request.Status = CodeReviewStatus.Failed;
            result.Summary = $"Review failed: {ex.Message}";
            result.QualityScore = 0;
        }

        return result;
    }

    public Task<CodeReviewStatus> GetReviewStatusAsync(string requestId)
    {
        if (_activeReviews.TryGetValue(requestId, out var request))
        {
            return Task.FromResult(request.Status);
        }
        return Task.FromResult(CodeReviewStatus.Pending);
    }

    public Task<List<CodeReviewRequest>> GetActiveReviewsAsync()
    {
        return Task.FromResult(_activeReviews.Values.ToList());
    }

    private async Task<Dictionary<string, string>> FetchCodeFilesAsync(CodeReviewRequest request)
    {
        // Simulate fetching code from repository
        // In a real implementation, this would integrate with Git providers
        var files = new Dictionary<string, string>();

        if (request.FilesToReview.Length == 0)
        {
            // Simulate some sample files for demo
            files["Program.cs"] = @"
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SampleApp
{
    public class Program
    {
        // TODO: Add proper error handling
        public static void Main(string[] args)
        {
            Console.WriteLine(""Starting application..."");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}";

            files["DatabaseService.cs"] = @"
using System.Data.SqlClient;

namespace SampleApp.Services
{
    public class DatabaseService
    {
        private string connectionString = ""Server=localhost;Database=MyApp;User Id=sa;Password=password123;"";
        
        public async Task<List<User>> GetUsersAsync(string name)
        {
            var users = new List<User>();
            var query = ""SELECT * FROM Users WHERE Name = '"" + name + ""'"";
            
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(query, connection);
            
            try
            {
                await connection.OpenAsync();
                var reader = command.ExecuteReader();
                // Process results...
            }
            catch (Exception ex)
            {
                // TODO: Log error
            }
            
            return users;
        }
    }
}";
        }
        else
        {
            foreach (var file in request.FilesToReview)
            {
                files[file] = $"// Simulated content for {file}";
            }
        }

        await Task.Delay(500); // Simulate network delay
        return files;
    }

    private int CalculateQualityScore(List<ReviewComment> comments)
    {
        if (comments.Count == 0) return 100;

        var score = 100;
        foreach (var comment in comments)
        {
            score -= comment.Severity switch
            {
                ReviewSeverity.Critical => 20,
                ReviewSeverity.Error => 10,
                ReviewSeverity.Warning => 5,
                ReviewSeverity.Info => 2,
                _ => 0
            };
        }

        return Math.Max(0, score);
    }

    private List<string> GenerateSuggestions(List<ReviewComment> comments)
    {
        var suggestions = new List<string>();
        
        if (comments.Any(c => c.Severity == ReviewSeverity.Critical))
            suggestions.Add("Address critical security vulnerabilities immediately");
            
        if (comments.Any(c => c.Message.Contains("TODO")))
            suggestions.Add("Complete TODO items before deployment");
            
        if (comments.Any(c => c.Message.Contains("exception")))
            suggestions.Add("Implement proper exception handling and logging");
            
        if (comments.Count > 10)
            suggestions.Add("Consider refactoring to reduce code complexity");

        return suggestions;
    }
}