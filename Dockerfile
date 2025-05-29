# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . .
RUN dotnet restore

# Build and publish the app
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published app from build stage
COPY --from=build /app/out ./

# Expose port 80
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "WeatherAPIWrapper.dll"]