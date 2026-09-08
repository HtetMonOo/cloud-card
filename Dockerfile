# =========================
# Build + Test stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project files first
# This allows Docker to cache the restore layer
COPY ["CloudCart.Api/CloudCart.Api.csproj", "CloudCart.Api/"]
COPY ["CloudCart.Tests/CloudCart.Tests.csproj", "CloudCart.Tests/"]

# Restore both projects
RUN dotnet restore "CloudCart.Api/CloudCart.Api.csproj"
RUN dotnet restore "CloudCart.Tests/CloudCart.Tests.csproj"

# Copy the source code
COPY . .

# Run unit tests
RUN dotnet test "CloudCart.Tests/CloudCart.Tests.csproj" \
    -c Release \
    --no-restore

# Publish API
RUN dotnet publish "CloudCart.Api/CloudCart.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CloudCart.Api.dll"]