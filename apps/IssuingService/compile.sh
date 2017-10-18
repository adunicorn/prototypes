set -e

unset DOCKER_HOST
unset DOCKER_TLS_VERIFY
unset DOCKER_API_VERSION

docker run -v $(pwd):/src mono nuget restore /src/IssuingService.csproj
docker run -v $(pwd):/src mono xbuild /src/IssuingService.csproj
