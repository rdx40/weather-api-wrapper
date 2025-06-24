# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest and build
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Optional: pass API key during build (injected via GitHub Actions)
ARG WEATHER_API_KEY
ENV WEATHER_API_KEY=${WEATHER_API_KEY}

COPY --from=build /app/out ./

EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "WeatherAPIWrapper.dll"]
