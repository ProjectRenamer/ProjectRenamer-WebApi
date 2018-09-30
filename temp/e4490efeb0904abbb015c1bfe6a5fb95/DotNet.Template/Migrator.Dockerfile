FROM microsoft/dotnet-nightly:2.1-sdk-alpine AS build-env

COPY . /DotNet.Template
WORKDIR /DotNet.Template/DotNet.Template.Migrator
RUN dotnet restore

RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /DotNet.Template/DotNet.Template.Migrator/out .

ENTRYPOINT ["dotnet", "DotNet.Template.Migrator.dll"]
