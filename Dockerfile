FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

#Improve layer caching and rebuild time
COPY *.csproj ./
RUN dotnet restore

# Copy everything and restore dependencies
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published app from build stage
COPY --from=build /app/out ./

# Expose port 80
EXPOSE 80

#Runtime environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV WEATHER_API_KEY=__REPLACE_AT_RUNTIME__

# Run the app
ENTRYPOINT ["dotnet", "WeatherAPIWrapper.dll"]
