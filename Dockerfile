FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update
RUN apt-get install ffmpeg -y
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ImageManipulation.Api/ImageManipulation.Api.csproj", "ImageManipulation.Api/"]
COPY ["ImageManipulation.Application/ImageManipulation.Application.csproj", "ImageManipulation.Application/"]
COPY ["ImageManipulation.Domain/ImageManipulation.Domain.csproj", "ImageManipulation.Domain/"]
COPY ["ImageManipulation.Infrastructure/ImageManipulation.Infrastructure.csproj", "ImageManipulation.Infrastructure/"]
RUN dotnet restore "ImageManipulation.Api/ImageManipulation.Api.csproj"
COPY . .
WORKDIR "/src/ImageManipulation.Api"
RUN dotnet build "ImageManipulation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImageManipulation.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageManipulation.Api.dll"]
