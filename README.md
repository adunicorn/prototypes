

## Teardown

`rmall`: stop and remove all services
`killall`: kill remove all container

### Operations

`docker service ls`: list the running services;
`docker service ps web`: list the running containers inside the service `web`
`docker ps`: list running containers
`docker ps -a`: list all container
`docker service scale web=42`: scale (up or down) the service `web` to `42` replicas (containers)

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
