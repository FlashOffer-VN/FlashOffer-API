# Dockerfile for FlashOffer.API.WebApi
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["FlashOffer.API.slnx", "."]
COPY ["src/FlashOffer.API.Domain/FlashOffer.API.Domain.csproj", "src/FlashOffer.API.Domain/"]
COPY ["src/FlashOffer.API.Application/FlashOffer.API.Application.csproj", "src/FlashOffer.API.Application/"]
COPY ["src/FlashOffer.API.Shared/FlashOffer.API.Shared.csproj", "src/FlashOffer.API.Shared/"]
COPY ["src/FlashOffer.API.Infrastructure/FlashOffer.API.Infrastructure.csproj", "src/FlashOffer.API.Infrastructure/"]
COPY ["src/FlashOffer.API.WebApi/FlashOffer.API.WebApi.csproj", "src/FlashOffer.API.WebApi/"]
COPY ["docs/FlashOffer.API.Documentation/FlashOffer.API.Documentation.csproj", "docs/FlashOffer.API.Documentation/"]
COPY ["tests/FlashOffer.API.UnitTests/FlashOffer.API.UnitTests.csproj", "tests/FlashOffer.API.UnitTests/"]
COPY ["tests/FlashOffer.API.IntegrationTests/FlashOffer.API.IntegrationTests.csproj", "tests/FlashOffer.API.IntegrationTests/"]

# Copy package management files
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

# Restore dependencies
RUN dotnet restore FlashOffer.API.slnx

# Copy all source code
COPY src/ src/
COPY docs/ docs/
COPY tests/ tests/

# Publish the WebApi project
RUN dotnet publish src/FlashOffer.API.WebApi/FlashOffer.API.WebApi.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published files
COPY --from=build /app/publish .

# Copy entrypoint script for Render PORT binding
COPY docker-entrypoint.sh .
RUN chmod +x docker-entrypoint.sh

# Create logs directory
RUN mkdir -p /app/logs

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_USE_POLLING_FILE_WATCHER=1

# Expose port (Render injects PORT at runtime)
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD sh -c 'curl -f http://localhost:${PORT:-8080}/health || exit 1'

# Run the application
ENTRYPOINT ["./docker-entrypoint.sh"]
