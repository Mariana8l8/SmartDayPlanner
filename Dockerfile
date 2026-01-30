FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY SmartDayPlanner/SmartDayPlanner.csproj SmartDayPlanner/
RUN dotnet restore SmartDayPlanner/SmartDayPlanner.csproj

COPY . .

WORKDIR /src/SmartDayPlanner

RUN dotnet publish "SmartDayPlanner.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "SmartDayPlanner.dll"]