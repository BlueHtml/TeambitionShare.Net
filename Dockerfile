# 使用微软官方 .NET 镜像作为构建环境
# https://hub.docker.com/_/microsoft-dotnet-sdk
FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS build
WORKDIR /app

# 将本地代码拷贝到容器内
COPY . ./
WORKDIR /app/TeambitionShare.Net/Server

# 构建项目
RUN dotnet publish -c Release -o out

# 使用微软官方 .NET 镜像作为运行时镜像
# https://hub.docker.com/_/microsoft-dotnet-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=build /app/TeambitionShare.Net/Server/out ./

# 启动服务
EXPOSE 5000
ENTRYPOINT ["dotnet", "Server.dll"]
