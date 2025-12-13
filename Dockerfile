# ------------------------
# Build Stage
# ------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy full project and publish
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ------------------------
# Runtime Stage
# ------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "IndoorLocalization.dll"]
