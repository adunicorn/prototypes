set -e

source ../docker-login.sh
docker login docker.io
reg=$(minishift openshift registry)

echo -e "\n\Publishing RabbitMQ Consumer image..."
docker tag $reg/issuing/consumer adunicorn/consumer
docker push adunicorn/consumer

echo -e "\n\Publishing the IssuingService image..."
docker tag $reg/issuing/issuing adunicorn/issuing
docker push adunicorn/issuing

echo -e "\n\Publishing the OldIssuingService image..."
docker tag $reg/issuing/old-issuing adunicorn/old-issuing
docker push adunicorn/old-issuing


echo -e "\n\nPublishing the Loader image..."
docker tag $reg/issuing/loader adunicorn/loader
docker push adunicorn/loader

