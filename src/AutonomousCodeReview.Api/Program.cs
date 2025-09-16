using AutonomousCodeReview.Core.Interfaces;
using AutonomousCodeReview.Core.Agents;
using AutonomousCodeReview.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Autonomous Code Review API", 
        Version = "v1",
        Description = "AI-powered code review system with multi-agent orchestration"
    });
});

// Register AI agents
builder.Services.AddSingleton<ICodeReviewAgent, SecurityReviewAgent>();
builder.Services.AddSingleton<ICodeReviewAgent, CodeQualityAgent>();

// Register orchestrator service
builder.Services.AddSingleton<IAgentOrchestrator, AgentOrchestratorService>();

// Add CORS for web client
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Autonomous Code Review API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at root
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowWebClient");
app.UseAuthorization();
app.MapControllers();

app.Run();
