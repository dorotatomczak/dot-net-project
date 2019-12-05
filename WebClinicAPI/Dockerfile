#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-nanoserver-1803 AS build
WORKDIR /src
COPY ["WebClinicAPI/WebClinicAPI.csproj", "WebClinicAPI/"]
RUN dotnet restore "WebClinicAPI/WebClinicAPI.csproj"
COPY . .
WORKDIR "/src/WebClinicAPI"
RUN dotnet build "WebClinicAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebClinicAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebClinicAPI.dll"]