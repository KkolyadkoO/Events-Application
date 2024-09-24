FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EventApp/EventApp.csproj", "EventApp/"]
COPY ["EventApp.Application/EventApp.Application.csproj", "EventApp.Application/"]
COPY ["EventApp.Core/EventApp.Core.csproj", "EventApp.Core/"]
COPY ["EventApp.DataAccess/EventApp.DataAccess.csproj", "EventApp.DataAccess/"]
COPY ["EventApp.Infrastructure/EventApp.Infrastructure.csproj", "EventApp.Infrastructure/"]
RUN dotnet restore "EventApp/EventApp.csproj"
COPY . .
WORKDIR "/src/EventApp"
RUN dotnet build "EventApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EventApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventApp.dll"]
