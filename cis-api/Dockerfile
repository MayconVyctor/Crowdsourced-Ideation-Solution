FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Cis.slnx Directory.Build.props global.json ./
COPY src/Cis.Core.Domain/Cis.Core.Domain.csproj src/Cis.Core.Domain/
COPY src/Cis.Core.Application/Cis.Core.Application.csproj src/Cis.Core.Application/
COPY src/Cis.Infrastructure.MySql/Cis.Infrastructure.MySql.csproj src/Cis.Infrastructure.MySql/
COPY src/Cis.Presentation/Cis.Presentation.csproj src/Cis.Presentation/
COPY src/Cis.Infrastructure.Ioc/Cis.Infrastructure.Ioc.csproj src/Cis.Infrastructure.Ioc/
COPY src/Cis.Server/Cis.Server.csproj src/Cis.Server/

RUN dotnet restore src/Cis.Server/Cis.Server.csproj

COPY src/ src/
RUN dotnet publish src/Cis.Server/Cis.Server.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:9001
EXPOSE 9001

ENTRYPOINT ["dotnet", "Cis.Server.dll"]
