FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SoftwareThingies.UI/SoftwareThingies.UI.csproj", "SoftwareThingies.UI/"]
COPY ["SoftwareThingies.UI.Client/SoftwareThingies.UI.Client.csproj", "SoftwareThingies.UI.Client/"]
RUN dotnet restore "SoftwareThingies.UI/SoftwareThingies.UI.csproj"
COPY . .
WORKDIR "/src/SoftwareThingies.UI"
RUN dotnet build "./SoftwareThingies.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SoftwareThingies.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoftwareThingies.UI.dll"]
