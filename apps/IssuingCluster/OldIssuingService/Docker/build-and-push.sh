source ../../docker-login.sh
reg=$(minishift openshift registry)

docker build -t $reg/issuing/old-issuing .
docker push $reg/issuing/old-issuing
