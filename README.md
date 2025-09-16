# Autonomous .NET Code Review

🤖 **AI-Powered Multi-Agent Code Review System**

The Autonomous .NET Code Review is an intelligent, automated code review system that leverages multiple specialized AI agents to analyze your code for security vulnerabilities, quality issues, and best practices. Built with .NET 8, this system provides both a robust API and a user-friendly web interface.

![Home Page](https://github.com/user-attachments/assets/a3f25588-8b47-4cf1-8ead-509c60c04c73)

## 🚀 Features

### Multi-Agent AI System
- **Security Review Agent**: Detects security vulnerabilities including SQL injection, hardcoded secrets, and potential attack vectors
- **Code Quality Agent**: Analyzes code for maintainability, best practices, naming conventions, and suggests improvements
- **AI Orchestrator**: Coordinates multiple agents to provide comprehensive analysis with intelligent prioritization

### User-Friendly Interface
- **Web Application**: Modern Blazor Server application with responsive design
- **Quick Review**: Paste code snippets for instant analysis
- **Repository Review**: Submit GitHub repositories for comprehensive analysis
- **Real-time Results**: Interactive results with quality scores and actionable suggestions

![Code Review Interface](https://github.com/user-attachments/assets/46b97cb5-4854-46c6-973d-55b87054dbe0)

### Robust API
- **RESTful API**: Well-documented API with OpenAPI/Swagger support
- **Multiple Endpoints**: Support for both repository and snippet analysis
- **Scalable Architecture**: Built for high-performance and scalability

## 🏗️ Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Blazor Web    │    │   ASP.NET Core  │    │      Core       │
│   Application   │◄──►│      API        │◄──►│    Library      │
│                 │    │                 │    │                 │
│  - Home Page    │    │  - Controllers  │    │  - AI Agents    │
│  - Code Review  │    │  - Swagger UI   │    │  - Models       │
│  - Interactive │    │  - CORS         │    │  - Orchestrator │
│    Interface    │    │  - Logging      │    │  - Interfaces   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 🛠️ Technology Stack

- **.NET 8**: Latest version of .NET for high performance
- **ASP.NET Core**: For building the API and web application
- **Blazor Server**: For interactive web UI
- **C#**: Primary programming language
- **Docker**: For containerization and deployment
- **GitHub Actions**: For CI/CD pipeline

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK
- Docker (optional, for containerized deployment)

### Running Locally

1. **Clone the repository**
   ```bash
   git clone https://github.com/saikumar2903/Autonomous-.NET-Code-Review.git
   cd Autonomous-.NET-Code-Review
   ```

2. **Build the solution**
   ```bash
   dotnet build
   ```

3. **Run the API** (Terminal 1)
   ```bash
   cd src/AutonomousCodeReview.Api
   dotnet run --urls "http://localhost:5000"
   ```

4. **Run the Web Application** (Terminal 2)
   ```bash
   cd src/AutonomousCodeReview.Web
   dotnet run --urls "http://localhost:5001"
   ```

5. **Access the applications**
   - Web Application: http://localhost:5001
   - API Documentation: http://localhost:5000

### Docker Deployment

1. **Build and run with Docker Compose**
   ```bash
   docker-compose up --build
   ```

2. **Access the applications**
   - Web Application: http://localhost:5001
   - API: http://localhost:5000

## 📖 API Documentation

The API provides comprehensive endpoints for code review automation:

### Core Endpoints

- `POST /api/codereview/submit` - Submit repository for review
- `GET /api/codereview/status/{requestId}` - Get review status
- `GET /api/codereview/active` - Get active reviews
- `POST /api/codereview/quick-review` - Quick code snippet review

### Example API Usage

```bash
# Submit a code review request
curl -X POST "http://localhost:5000/api/codereview/submit" \
  -H "Content-Type: application/json" \
  -d '{
    "repositoryUrl": "https://github.com/user/repo",
    "branchName": "main",
    "filesToReview": ["src/Program.cs", "src/Services/DatabaseService.cs"]
  }'

# Quick review a code snippet
curl -X POST "http://localhost:5000/api/codereview/quick-review" \
  -H "Content-Type: application/json" \
  -d '{
    "fileName": "example.cs",
    "code": "public class Example { /* your code here */ }"
  }'
```

## 🔧 Configuration

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Set to `Development` or `Production`
- `ASPNETCORE_URLS`: URLs the application should listen on
- `ApiBaseUrl`: Base URL for API calls (Web app only)

### Logging

The application uses ASP.NET Core's built-in logging system. Configure logging levels in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 🧪 Testing

Run the test suite:

```bash
dotnet test
```

## 🚀 Deployment

### GitHub Actions

The repository includes a complete CI/CD pipeline with GitHub Actions that:
- Builds and tests the application
- Creates Docker images
- Pushes to GitHub Container Registry
- Supports automatic deployment

### Production Deployment

1. **Using Docker Images**
   ```bash
   docker pull ghcr.io/saikumar2903/autonomous-.net-code-review-web:latest
   docker pull ghcr.io/saikumar2903/autonomous-.net-code-review-api:latest
   ```

2. **Cloud Deployment**
   - Azure Container Instances
   - AWS ECS
   - Google Cloud Run
   - Kubernetes

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔮 Future Enhancements

- Integration with popular Git providers (GitHub, GitLab, Azure DevOps)
- Additional AI agents for performance analysis
- Machine learning model training on custom codebases
- Integration with IDEs and code editors
- Advanced reporting and analytics
- Team collaboration features

## 📞 Support

For support, email support@autonomouscodereview.com or create an issue in the GitHub repository.

---

**Built with ❤️ and AI by the Autonomous Code Review Team**
