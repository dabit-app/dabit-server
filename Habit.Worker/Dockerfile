FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Habit.Worker/Habit.Worker.csproj", "Habit.Worker/"]
RUN dotnet restore "Habit.Worker/Habit.Worker.csproj"
COPY . .
WORKDIR "/src/Habit.Worker"
RUN dotnet build "Habit.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Habit.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Habit.Worker.dll"]
