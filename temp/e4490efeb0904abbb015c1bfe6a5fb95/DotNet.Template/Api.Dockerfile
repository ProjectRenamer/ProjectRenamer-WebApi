FROM microsoft/dotnet-nightly:2.1-sdk-alpine AS build-env

COPY . /DotNet.Template
WORKDIR /DotNet.Template/DotNet.Template.Api
RUN dotnet restore

RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /DotNet.Template/DotNet.Template.Api/out .

EXPOSE 80
HEALTHCHECK --interval=5s --timeout=3s --retries=3 CMD curl -f / http://localhost:80/health-check || exit 1 
ENTRYPOINT ["dotnet", "DotNet.Template.Api.dll"]
