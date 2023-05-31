#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TransactionApp/TransactionApp.csproj", "TransactionApp/"]
COPY ["TransactionApp.Application/TransactionApp.Application.csproj", "TransactionApp.Application/"]
COPY ["TransactionApp.Domain/TransactionApp.Domain.csproj", "TransactionApp.Domain/"]
COPY ["TransactionApp.Infrastructure/TransactionApp.Infrastructure.csproj", "TransactionApp.Infrastructure/"]
RUN dotnet restore "TransactionApp/TransactionApp.csproj"
COPY . .
WORKDIR "/src/TransactionApp"
RUN dotnet build "TransactionApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransactionApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionApp.dll"]