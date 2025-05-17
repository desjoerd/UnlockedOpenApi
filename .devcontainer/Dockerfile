FROM mcr.microsoft.com/devcontainers/dotnet:1-8.0

COPY --from=mcr.microsoft.com/dotnet/sdk:9.0 /usr/share/dotnet/ /usr/share/dotnet/
COPY --from=mcr.microsoft.com/dotnet/sdk:10.0.100-preview.4 /usr/share/dotnet/ /usr/share/dotnet/

# Install dependencies for nvm and Node.js
RUN apt-get update && apt-get install -y curl ca-certificates gnupg \
  && rm -rf /var/lib/apt/lists/*

# Install Node.js 22.x
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash - \
    && apt-get update \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*
