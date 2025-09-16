using AutonomousCodeReview.Web.Components;
using AutonomousCodeReview.Core.Interfaces;
using AutonomousCodeReview.Core.Agents;
using AutonomousCodeReview.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register AI agents
builder.Services.AddSingleton<ICodeReviewAgent, SecurityReviewAgent>();
builder.Services.AddSingleton<ICodeReviewAgent, CodeQualityAgent>();

// Register orchestrator service
builder.Services.AddSingleton<IAgentOrchestrator, AgentOrchestratorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
