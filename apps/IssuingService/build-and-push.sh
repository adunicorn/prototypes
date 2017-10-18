source ../../docker-login.sh
reg=$(minishift openshift registry)

docker build -t $reg/issuing/issuing .
docker push $reg/issuing/issuing
