FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY BigBangAPI/BigBangAPI.csproj BigBangAPI/
RUN dotnet restore BigBangAPI/BigBangAPI.csproj

COPY BigBangAPI/ BigBangAPI/
RUN dotnet publish BigBangAPI/BigBangAPI.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "BigBangAPI.dll"]
