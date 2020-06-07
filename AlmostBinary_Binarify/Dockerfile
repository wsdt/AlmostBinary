FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AlmostBinary_Binarify/AlmostBinary_Binarify.csproj", "AlmostBinary_Binarify/"]
COPY ["AlmostBinary_GlobalConstants/AlmostBinary_GlobalConstants.csproj", "AlmostBinary_GlobalConstants/"]
RUN dotnet restore "AlmostBinary_Binarify/AlmostBinary_Binarify.csproj"
COPY . .
WORKDIR "/src/AlmostBinary_Binarify"
RUN dotnet build "AlmostBinary_Binarify.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlmostBinary_Binarify.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlmostBinary_Binarify.dll"]