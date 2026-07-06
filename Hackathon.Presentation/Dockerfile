FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Hackathon.Presentation/Hackathon.Presentation.csproj", "Hackathon.Presentation/"]
COPY ["Hackathon.Application/Hackathon.Application.csproj", "Hackathon.Application/"]
COPY ["Hackathon.Domain/Hackathon.Domain.csproj", "Hackathon.Domain/"]
COPY ["Hackathon.Infrastructure/Hackathon.Infrastructure.csproj", "Hackathon.Infrastructure/"]
RUN dotnet restore "Hackathon.Presentation/Hackathon.Presentation.csproj"
COPY . .
WORKDIR "/src/Hackathon.Presentation"
RUN dotnet build "./Hackathon.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hackathon.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hackathon.Presentation.dll"]
