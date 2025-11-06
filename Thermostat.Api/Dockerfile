# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copy project files (layered for caching)
COPY ["Thermostat/Thermostat.csproj", "Thermostat/"]
COPY ["Thermostat.Api/Thermostat.Api.csproj", "Thermostat.Api/"]
# Fixed paths - match your actual nested structure
COPY ["Thermostat.Domain/Thermostat.Domain/Thermostat.Domain.csproj", "Thermostat.Domain/Thermostat.Domain/"]
COPY ["Thermostat.DataAccessLayer/Thermostat.DataAccessLayer/Thermostat.DataAccessLayer.csproj", "Thermostat.DataAccessLayer/Thermostat.DataAccessLayer/"]

# Restore dependencies
RUN dotnet restore "Thermostat.Api/Thermostat.Api.csproj"

# Copy all source code
COPY . .

# Build & publish
WORKDIR "/src/Thermostat.Api"
RUN dotnet publish "Thermostat.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Thermostat.Api.dll"]