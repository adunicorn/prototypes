#!/bin/bash
set -e

unset DOCKER_HOST
unset DOCKER_TLS_VERIFY
unset DOCKER_API_VERSION

docker run --rm -v $(pwd):/src mono nuget restore /src/Client.sln
docker run --rm -v $(pwd):/src mono msbuild /src/Client.sln
