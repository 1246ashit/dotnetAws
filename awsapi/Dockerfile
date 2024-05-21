# 使用官方的 .NET Core SDK 作為基礎映像檔
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 複製專案檔案並還原任何依賴（使用 .csproj 作為專案名稱）
COPY *.csproj ./
RUN dotnet restore

# 複製應用程式的其餘部分
COPY . .

# 發佈應用程式
RUN dotnet publish -c Release -o out

# 建立運行時映像檔
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# 暴露應用程式將使用的埠
EXPOSE 80

# 啟動應用程式
ENTRYPOINT ["dotnet", "awsapi.dll"]
