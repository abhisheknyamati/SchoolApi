FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5181
ENV ASPNETCORE_ENVIRONMENT=Development
USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Debug
WORKDIR /src
COPY ["src/SchoolProject.ApiGateway/SchoolProject.ApiGateway.csproj", "src/SchoolProject.ApiGateway/"]
RUN dotnet restore "src/SchoolProject.ApiGateway/SchoolProject.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/SchoolProject.ApiGateway"
RUN dotnet build "SchoolProject.ApiGateway.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Debug
RUN dotnet publish "SchoolProject.ApiGateway.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
EXPOSE 5181
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchoolProject.ApiGateway.dll"]
