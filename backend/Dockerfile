# Estágio de Base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Estágio de Build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar arquivos de projeto primeiro para cache de restauração
COPY ["CalculadoraCalorias.Api/CalculadoraCalorias.Api.csproj", "CalculadoraCalorias.Api/"]
COPY ["CalculadoraCalorias.Application/CalculadoraCalorias.Application.csproj", "CalculadoraCalorias.Application/"]
COPY ["CalculadoraCalorias.Core/CalculadoraCalorias.Core.csproj", "CalculadoraCalorias.Core/"]
COPY ["CalculadoraCalorias.Infrastructure/CalculadoraCalorias.Infrastructure.csproj", "CalculadoraCalorias.Infrastructure/"]

RUN dotnet restore "CalculadoraCalorias.Api/CalculadoraCalorias.Api.csproj"

# Copiar o restante do código
COPY . .
WORKDIR "/src/CalculadoraCalorias.Api"
RUN dotnet build "CalculadoraCalorias.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CalculadoraCalorias.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/wwwroot/uploads/refeicoes
ENTRYPOINT ["dotnet", "CalculadoraCalorias.Api.dll"]
