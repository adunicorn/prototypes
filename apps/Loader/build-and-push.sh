reg=$(minishift openshift registry)

docker build -t $reg/issuing/loader .
docker push $reg/issuing/loader
