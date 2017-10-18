set -e

unset DOCKER_HOST
unset DOCKER_TLS_VERIFY
unset DOCKER_API_VERSION

docker run -h localhost -v $(pwd)/src/:/src mono nuget restore /src/RabbitConsumer.sln
docker run -h localhost -v $(pwd)/src/:/src mono msbuild /src/RabbitConsumer.sln


