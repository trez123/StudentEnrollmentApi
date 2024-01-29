FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["StudentEnrollmentFrontend.csproj", "."]
RUN dotnet restore "./StudentEnrollmentFrontend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "StudentEnrollmentFrontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudentEnrollmentFrontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentEnrollmentFrontend.dll"]