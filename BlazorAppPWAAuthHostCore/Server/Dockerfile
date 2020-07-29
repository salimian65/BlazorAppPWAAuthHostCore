#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BlazorAppPWAAuthHostCore/Server/BlazorAppPWAAuthHostCore.Server.csproj", "BlazorAppPWAAuthHostCore/Server/"]
COPY ["BlazorAppPWAAuthHostCore/Client/BlazorAppPWAAuthHostCore.Client.csproj", "BlazorAppPWAAuthHostCore/Client/"]
COPY ["BlazorAppPWAAuthHostCore/Shared/BlazorAppPWAAuthHostCore.Shared.csproj", "BlazorAppPWAAuthHostCore/Shared/"]
COPY ["BlazorAppPWAAuthHostCore/EmojiPicker/MyBlazerChatSample.EmojiPicker.csproj", "BlazorAppPWAAuthHostCore/EmojiPicker/"]
RUN dotnet restore "BlazorAppPWAAuthHostCore/Server/BlazorAppPWAAuthHostCore.Server.csproj"
COPY . .
WORKDIR "/src/BlazorAppPWAAuthHostCore/Server"
#RUN dotnet build "BlazorAppPWAAuthHostCore.Server.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "BlazorAppPWAAuthHostCore.Server.csproj" -c Release -o /app/publish

RUN ../../build.sp1

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorAppPWAAuthHostCore.Server.dll"]