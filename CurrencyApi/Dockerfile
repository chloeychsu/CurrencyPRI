FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# runtime image
FROM mcr.microsoft.com/dotnet/sdk:8.0   
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","CurrencyApi.dll" ]