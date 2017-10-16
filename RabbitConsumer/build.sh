set -e

docker run -v $(pwd)/src/:/src mono nuget restore /src/RabbitConsumer.sln
docker run -v $(pwd)/src/:/src mono msbuild /src/RabbitConsumer.sln

docker build -t adunicorn/consumer .

