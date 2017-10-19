set -e

oc login -u admin # -p OriginAdmin

PROJECT_NAME=old-issuing
echo "\n\n\n** Deleting the project ${PROJECT_NAME}.."
oc delete project ${PROJECT_NAME}
