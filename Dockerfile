FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
ADD ./CA.cer /usr/local/share/ca-certificates/CA.crt
RUN chmod 644 /usr/local/share/ca-certificates/CA.crt && update-ca-certificates && c_rehash
RUN apt-get install -y curl
RUN curl -v https://api.nuget.org/v3/index.json
RUN dotnet restore
COPY . .
WORKDIR "/src/."
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "DotnetProject.SampleApi.Api.dll"]
