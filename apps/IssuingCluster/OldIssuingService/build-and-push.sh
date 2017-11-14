reg=$(minishift openshift registry)

docker build -t $reg/old-issuing/old-issuing .
docker push $reg/old-issuing/old-issuing
