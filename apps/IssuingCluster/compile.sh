#!/bin/bash
set -e

unset DOCKER_HOST
unset DOCKER_TLS_VERIFY
unset DOCKER_API_VERSION

docker run -v $(pwd):/src mono nuget restore /src/IssuingCluster.sln
docker run -v $(pwd):/src mono msbuild /src/IssuingCluster.sln
