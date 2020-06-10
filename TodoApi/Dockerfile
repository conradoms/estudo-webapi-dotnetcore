FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

COPY TodoApi/bin/release/netcoreapp3.1/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "TodoApi.dll"]