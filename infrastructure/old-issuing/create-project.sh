set -e

oc login -u admin # -p OriginAdmin

PROJECT_NAME=old-issuing
echo "\n\n\n** Creating the project ${PROJECT_NAME}.."
oc new-project ${PROJECT_NAME}

echo "\n\n\n** Importing the docker images"
oc import-image old-issuing --from=docker.io/adunicorn/old-issuing --confirm
oc import-image postgresql:9.5 --from=docker.io/centos/postgresql-95-centos7 --confirm
oc import-image loader --from=docker.io/adunicorn/loader --confirm

# Loader
oc create -f openshift-resources/loader-deployment-config.yml

## PostgreSQL
oc create -f openshift-resources/postgresql-deployment-config.yml
oc create -f openshift-resources/postgresql-service.yml
#oc expose dc postgresql --type=LoadBalancer --name=postgresql-ingress
oc create -f openshift-resources/postgresql-ingress.yml

## Old Issuing
oc create -f openshift-resources/old-issuing-deployment-config.yml
oc create -f openshift-resources/old-issuing-service.yml
oc create -f openshift-resources/old-issuing-route.yml
oc create -f openshift-resources/old-issuing-minishift-route.yml

echo "\n\n\n** Deploying..."
#oc deploy old-issuing
#oc deploy rabbitmq


