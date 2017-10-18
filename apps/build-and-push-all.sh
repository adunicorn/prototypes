set -e

source ../docker-login.sh

echo -e "\n\nBuilding RabbitMQ Consumer image..."
(cd RabbitConsumer && ./build-and-push.sh)


echo -e "\n\nBuilding the IssuingService image..."
(cd IssuingService && ./build-and-push.sh)

echo -e "\n\nBuilding the OldIssuingService image..."
(cd OldIssuingService && ./build-and-push.sh)

echo -e "\n\n"Building the Loader image..."
(cd Loader && ./build-and.push.sh)
