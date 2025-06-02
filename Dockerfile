FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 9096

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PasswordBackendChallenge.Presentation/PasswordBackendChallenge.Presentation.csproj", "PasswordBackendChallenge.Presentation/"]
RUN dotnet restore "PasswordBackendChallenge.Presentation/PasswordBackendChallenge.Presentation.csproj"
COPY ./src .
WORKDIR "/src/PasswordBackendChallenge.Presentation"
RUN dotnet build "./PasswordBackendChallenge.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PasswordBackendChallenge.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PasswordBackendChallenge.Presentation.dll"]