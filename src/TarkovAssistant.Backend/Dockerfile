FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
RUN apk add --no-cache krb5-libs
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY ["src/TarkovAssistant.Backend/TarkovAssistant.Backend.csproj", "src/TarkovAssistant.Backend/"]
RUN dotnet restore "src/TarkovAssistant.Backend/TarkovAssistant.Backend.csproj"
COPY . .
RUN dotnet publish "src/TarkovAssistant.Backend/TarkovAssistant.Backend.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "TarkovAssistant.Backend.dll"]
