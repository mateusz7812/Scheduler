FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app
RUN git clone https://github.com/mateusz7812/Scheduler.git
WORKDIR /app/Scheduler/SchedulerWebApplication
RUN dotnet restore
RUN dotnet publish -c Debug -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/Scheduler/SchedulerWebApplication/out .
ENTRYPOINT ["dotnet", "SchedulerWebApplication.dll"]