FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS builder
WORKDIR /app
COPY ./ .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS runner
WORKDIR /app
COPY --from=builder /app/out ./
RUN apk add icu-libs=73.2-r2 --no-cache
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV COMPlus_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "SendoraCityApi.dll"]
