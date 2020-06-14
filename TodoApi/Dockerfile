# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
LABEL version="1.0" maintainer="Conrado"
WORKDIR /app
COPY ./dist .

ENTRYPOINT ["dotnet", "TodoApi.dll"]