FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BE-SEAL-HACKATHON.Presentation/BE-SEAL-HACKATHON.Presentation.csproj", "BE-SEAL-HACKATHON.Presentation/"]
COPY ["BE-SEAL-HACKATHON.Infrastructure/BE-SEAL-HACKATHON.Infrastructure.csproj", "BE-SEAL-HACKATHON.Infrastructure/"]
COPY ["BE-SEAL-HACKATHON.Application/BE-SEAL-HACKATHON.Application.csproj", "BE-SEAL-HACKATHON.Application/"]
COPY ["BE-SEAL-HACKATHON.Domain/BE-SEAL-HACKATHON.Domain.csproj", "BE-SEAL-HACKATHON.Domain/"]
RUN dotnet restore "BE-SEAL-HACKATHON.Presentation/BE-SEAL-HACKATHON.Presentation.csproj"
COPY . .
WORKDIR "/src/BE-SEAL-HACKATHON.Presentation"
RUN dotnet build "BE-SEAL-HACKATHON.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BE-SEAL-HACKATHON.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BE-SEAL-HACKATHON.Presentation.dll"]
