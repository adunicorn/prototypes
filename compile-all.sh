set -e
source docker-use-local.sh
echo "Compiling IssuingCluster solution..."
(cd apps/IssuingCluster && ./compile.sh)
