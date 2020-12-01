param (
    [string]$nugetVersion
)

dotnet publish "uCAN.sln" -c Release
dotnet pack --include-source --no-build -c Release -p:PackageVersion=$nugetVersion -o "." "uCAN.NetStandard\uCAN.NetStandard.csproj"