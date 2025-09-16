# Autonomous .NET Code Review - Deployment Guide

## Quick Start

### Development Mode
```bash
# Clone the repository
git clone https://github.com/saikumar2903/Autonomous-.NET-Code-Review.git
cd Autonomous-.NET-Code-Review

# Build and test
dotnet build
dotnet test

# Run API (Terminal 1)
cd src/AutonomousCodeReview.Api
dotnet run --urls "http://localhost:5000"

# Run Web App (Terminal 2)
cd src/AutonomousCodeReview.Web
dotnet run --urls "http://localhost:5001"
```

### Production Deployment with Docker
```bash
# Build and run with Docker Compose
docker-compose up --build -d

# Access applications
# Web: http://localhost:5001
# API: http://localhost:5000
```

### Cloud Deployment

#### Azure Container Instances
```bash
# Build images
docker build -f Dockerfile.Api -t autonomouscodereview-api .
docker build -f Dockerfile.Web -t autonomouscodereview-web .

# Tag for Azure Container Registry
docker tag autonomouscodereview-api youracr.azurecr.io/autonomouscodereview-api:latest
docker tag autonomouscodereview-web youracr.azurecr.io/autonomouscodereview-web:latest

# Push to registry
docker push youracr.azurecr.io/autonomouscodereview-api:latest
docker push youracr.azurecr.io/autonomouscodereview-web:latest

# Deploy to Azure Container Instances
az container create --resource-group myResourceGroup \
  --name autonomouscodereview-api \
  --image youracr.azurecr.io/autonomouscodereview-api:latest \
  --ports 80 --ip-address public
```

#### AWS ECS / Google Cloud Run
Similar deployment patterns using respective cloud services.

### Environment Variables

#### API
- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ASPNETCORE_URLS`: http://+:8080

#### Web Application
- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ASPNETCORE_URLS`: http://+:8080
- `ApiBaseUrl`: http://api-service:8080 (for container deployments)

### Health Checks
- API Health: `GET /health`
- Web Health: `GET /health`
- Swagger UI: `GET /swagger`

### Security Considerations
- Enable HTTPS in production
- Configure proper CORS policies
- Set up authentication/authorization if needed
- Use secure secrets management
- Regular security updates

### Monitoring
- Application logs via ASP.NET Core logging
- Health check endpoints for monitoring
- Docker health checks configured
- Metrics collection through Application Insights (optional)

### Scaling
- Horizontal scaling: Deploy multiple container instances
- Load balancing: Use Azure Load Balancer, AWS ALB, or similar
- Database: Consider adding persistent storage for results
- Caching: Redis for improved performance