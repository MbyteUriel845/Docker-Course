# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el csproj y restaurar
COPY DemoDockerAPI.csproj .
RUN dotnet restore

# Copiar el resto del proyecto
COPY . .

# Crear carpeta explícitamente para publicar
RUN mkdir -p /src/publish

# Publicar y mostrar contenido de la carpeta publish
RUN dotnet publish -c Release -o /src/publish
RUN ls -la /src/publish

# Etapa final runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar la publicación desde la etapa build
COPY --from=build /src/publish .

ENTRYPOINT ["dotnet", "DemoDockerAPI.dll"]
