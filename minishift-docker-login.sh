eval $(minishift docker-env)
docker login $(minishift openshift registry) -u admin -p $(oc whoami -t)
