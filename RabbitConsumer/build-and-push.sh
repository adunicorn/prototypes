reg=$(minishift openshift registry)

docker build -t $reg/issuing/consumer .
docker push $reg/issuing/consumer
