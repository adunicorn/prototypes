set -e

oc login -u admin # -p OriginAdmin

PROJECT_NAME=issuing
echo "\n\n\n** Creating the project ${PROJECT_NAME}.."
oc new-project ${PROJECT_NAME}
oc project issuing

echo "\n\n\n** Importing the docker images"
oc import-image redis --from=docker.io/bitnami/redis --confirm
oc import-image issuing --from=docker.io/adunicorn/issuing --confirm
oc import-image consumer --from=docker.io/adunicorn/consumer --confirm
oc import-image rabbitmq --from=docker.io/luiscoms/openshift-rabbitmq --all --confirm
oc import-image postgresql:9.5 --from=docker.io/centos/postgresql-95-centos7 --confirm
oc import-image loader --from=docker.io/adunicorn/loader --confirm

# Loader
oc create -f openshift-resources/loader-deployment-config.yml

echo "\n\n\n** Creating OpenShift resources from exports.."
#oc create -f openshift-templates/redis-ephemeral-template.json

## PostgreSQL
oc create -f openshift-resources/postgresql-deployment-config.yml
oc create -f openshift-resources/postgresql-service.yml
#oc expose dc postgresql --type=LoadBalancer --name=postgresql-ingress
oc create -f openshift-resources/postgresql-ingress.yml

## Redis
oc create -f openshift-resources/redis-master-deployment-config.yml
oc create -f openshift-resources/redis-master-service.yml

## Issuing
oc create -f openshift-resources/issuing-deployment-config.yml
oc create -f openshift-resources/issuing-service.yml
oc create -f openshift-resources/issuing-route.yml
oc create -f openshift-resources/issuing-minishift-route.yml

## Consumer
oc create -f openshift-resources/consumer-deployment-config.yml
oc create -f openshift-resources/consumer-service.yml


## RabbitMQ
oc create -f openshift-resources/rabbitmq-deployment-config.yml
oc create -f openshift-resources/rabbitmq-service.yml
oc create -f openshift-resources/rabbitmq-route.yml
oc create -f openshift-resources/rabbitmq-minishift-route.yml

echo "\n\n\n** Deploying..."
oc deploy redis-master
oc deploy issuing
oc deploy rabbitmq


