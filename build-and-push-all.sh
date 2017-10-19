set -e

source minishift-docker-login.sh

echo -e "\n\nBuilding RabbitMQ Consumer image..."
(cd apps/IssuingCluster/RabbitConsumer && ./build-and-push.sh)

echo -e "\n\nBuilding the IssuingService image..."
(cd apps/IssuingCluster/IssuingService && ./build-and-push.sh)

echo -e "\n\nBuilding the Loader image..."
(cd apps/Loader && ./build-and-push.sh)

echo -e "\n\nBuilding the OldIssuingService image..."
(cd apps/IssuingCluster/OldIssuingService && ./build-and-push.sh)

echo Done
