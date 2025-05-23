# Etapa 1: build do projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore IntegreBackend.csproj
RUN dotnet publish IntegreBackend.csproj -c Release -o /app/publish

# Etapa 2: runtime da aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IntegreBackend.dll"]
