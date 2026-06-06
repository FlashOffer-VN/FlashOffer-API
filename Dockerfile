# Dockerfile for DotnetApiBase.WebApi
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["DotnetApiBase.slnx", "."]
COPY ["src/DotnetApiBase.Domain/DotnetApiBase.Domain.csproj", "src/DotnetApiBase.Domain/"]
COPY ["src/DotnetApiBase.Application/DotnetApiBase.Application.csproj", "src/DotnetApiBase.Application/"]
COPY ["src/DotnetApiBase.Shared/DotnetApiBase.Shared.csproj", "src/DotnetApiBase.Shared/"]
COPY ["src/DotnetApiBase.Infrastructure/DotnetApiBase.Infrastructure.csproj", "src/DotnetApiBase.Infrastructure/"]
COPY ["src/DotnetApiBase.WebApi/DotnetApiBase.WebApi.csproj", "src/DotnetApiBase.WebApi/"]
COPY ["docs/DotnetApiBase.Documentation/DotnetApiBase.Documentation.csproj", "docs/DotnetApiBase.Documentation/"]
COPY ["tests/DotnetApiBase.UnitTests/DotnetApiBase.UnitTests.csproj", "tests/DotnetApiBase.UnitTests/"]
COPY ["tests/DotnetApiBase.IntegrationTests/DotnetApiBase.IntegrationTests.csproj", "tests/DotnetApiBase.IntegrationTests/"]

# Copy package management files
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

# Restore dependencies
RUN dotnet restore DotnetApiBase.slnx

# Copy all source code
COPY src/ src/
COPY docs/ docs/
COPY tests/ tests/

# Publish the WebApi project
RUN dotnet publish src/DotnetApiBase.WebApi/DotnetApiBase.WebApi.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published files
COPY --from=build /app/publish .

# Create logs directory
RUN mkdir -p /app/logs

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_USE_POLLING_FILE_WATCHER=1

# Expose ports
EXPOSE 80
EXPOSE 443

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "DotnetApiBase.WebApi.dll"]
