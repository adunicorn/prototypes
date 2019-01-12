# Prototypes

Disposable prototypes for our experiments


# Run presentation

See [Setup](#setup) for setting the environment up. 

* Start Docker in macOS
* `cd ~prg/openshift/prototypes`
* `minishift start`
* If `pyissuing` is failing, login into the application `Loader`, open a terminal and run `/app/create.sh` to load data into Redis
* In a separate Shell run `cd ./apps/client && ./run.sh 2> /dev/null&`
* `oc login` (admin/admin)
* `source ./minishift-docker-login.sh`
* `cd ./apps/IssuingCluster/pyissuing`
* Modify the code as desired, then deploy it with `./build-and-push.sh`


## Setup

* Install Docker;
* Setup a `/work` shared folded on VirtualBox VM poiting to the directory containing this project
* Install MiniShift and start it with

```bash
minishift start
```

The console should be available at:

```bash
https://192.168.64.11:8443
```

with


```bash
username: admin
password: admin
```


## Create the project issuing

Make sure that no previous issuing project exists.

* Delete the project in OpenShift with

```bash
cd infrastructure
./delete-project.sh
```

`oc projects` should not list the project issuing anymore.


* Create the project with

```bash
./create-projects.sh
```


that should take care of all the process, creating all the needed elements in OpenShift.

Opening the dashboard at a URL such as https://192.168.64.11:8443/console/project/issuing/overview it should be possible to see the project deployments running.

Wait ta minute to let all the deployment tasks end.
Compile the Windows Forms client

* Start Docker

* Enter the client project with

```bash
cd apps/Client
```


* Compile the client with

```bash
./compile.sh
```


* Run the client with

```bash
./run.sh
```


## Build

Run

```bash
build.bat
```

in IssuingService and RabbitConsumer.

## Run

Run

```bash
run.bat
```


## Teardown

`rmall`: stop and remove all services
`killall`: kill remove all container

### Operations

* `docker service ls`: list the running services;
* `docker service ps web`: list the running containers inside the service `web`
* `docker ps`: list running containers
* `docker ps -a`: list all container
* `docker service scale web=42`: scale (up or down) the service `web` to `42` replicas (containers)

## Usage

Use the API, visiting:

`[GET] /api/cardholders/counter` to count the persisted card holders

`[POST] api/cardholder` to create a card holder
`[GET] api/cardholder/{id}` to read a card holder


The `[GET]` call `api/cardholder/1` is pre-populated and should return:

```xml
<CardHolder>
  <ID>1</ID>
  <Firstname>Marco</Firstname>
  <Lastname>Bernasconi</Lastname>
</CardHolder>
```
