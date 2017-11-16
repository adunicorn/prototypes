reg=$(minishift openshift registry)

docker build -t $reg/issuing/pyissuing .
docker push $reg/issuing/pyissuing
