FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /build

COPY ./src ./src

RUN dotnet restore src/Cdb.Calculator.Api/Cdb.Calculator.Api.csproj
RUN dotnet publish src/Cdb.Calculator.Api/Cdb.Calculator.Api.csproj -c Release -o src/Cdb.Calculator.Api/dist

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build-env /build/src/Cdb.Calculator.Api/dist ./

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://*:80
EXPOSE 80

USER app

ENTRYPOINT ["dotnet", "Cdb.Calculator.Api.dll"]