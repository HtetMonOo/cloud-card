# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["CloudCart.Api.csproj", "./"]
RUN dotnet restore "CloudCart.Api.csproj"

# Copy everything and publish
COPY . .
RUN dotnet publish "CloudCart.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CloudCart.Api.dll"]
