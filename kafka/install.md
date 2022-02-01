# install kafka and kafka-ui

`kubectl create namespace kafka` 

Add helm repo: 
```
helm repo add bitnami https://charts.bitnami.com/bitnami
``` 

## kafka:

```
helm install kafka bitnami/kafka \
  --set zookeeper.enabled=true \
  --set replicaCount=1 -n kafka
```

## kafka-ui:
```
helm repo add kafka-ui https://provectus.github.io/kafka-ui
helm install kafka-ui kafka-ui/kafka-ui --set envs.config.KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS=kafka.kafka:9092 --set envs.config.KAFKA_CLUSTERS_0_ZOOKEEPER=kafka-zookeeper.kafka:2181 --set image.tag=0.3.2 -n kafka
```

Access kafka-ui with port-forward

```
kubectl port-forward service/kafka-ui -n kafka 8080:80
```
