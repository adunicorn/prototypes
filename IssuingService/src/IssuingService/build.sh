set -e

docker run -v $(pwd):/src mono nuget restore /src/IssuingService.csproj
docker run -v $(pwd):/src mono xbuild /src/IssuingService.csproj

docker build -t adunicorn/issuing .
