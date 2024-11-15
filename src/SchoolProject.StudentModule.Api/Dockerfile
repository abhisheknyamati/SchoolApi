FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5207
ENV ASPNETCORE_ENVIRONMENT=Development
USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Debug
WORKDIR /src
COPY ["src/SchoolProject.StudentModule.Api/SchoolProject.StudentModule.Api.csproj", "src/SchoolProject.StudentModule.Api/"]
RUN dotnet restore "src/SchoolProject.StudentModule.Api/SchoolProject.StudentModule.Api.csproj"
COPY . .
WORKDIR "/src/src/SchoolProject.StudentModule.Api"
RUN dotnet build "SchoolProject.StudentModule.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Debug
RUN dotnet publish "SchoolProject.StudentModule.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
EXPOSE 5207
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchoolProject.StudentModule.Api.dll"]
