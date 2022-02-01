# Start Producer

Access kafka-ui in `http://localhost:8080` and create a `fruits` topic with 10 partitions.  

Install the producer deployment.

```
kubectl apply -f deploy-producer.yaml
```
Open a service port with port-forward

```
kubectl port-forward service/producer-service 8081:80
```
Generate load on producer. 

```
xargs -I % -P 20 curl -X GET "localhost:8081/process/10" < <(printf '%s\n' {1..4000})
``` 
- pralalelism degree: `-P 20` 
- quantity per request: 10 `localhost:8081/process/10`
- quantity of calls: `{1..4000}`
- to stop load `ctrl+c`
# Start Consumer

```
kubectl apply -f deploy-consumer.yaml
```

Start the Keda object to scale de consumer-deployment, one HPA will be created to take care of scale based on consumer-group lag 

```yaml
apiVersion: keda.sh/v1alpha1
kind: ScaledObject
metadata:
  name: consumer-fruit-kafka-scaledobject
  namespace: default
spec:
  scaleTargetRef:
    name: consumer-deployment
  cooldownPeriod: 60
  pollingInterval: 20
  maxReplicaCount: 10
  triggers:
  - type: kafka
    metadata:
      bootstrapServers: kafka.kafka:9092
      consumerGroup: fruit-processing       # Make sure that this consumer group name is the same one as the one that is consuming topics
      topic: fruits
      # Optional
      lagThreshold: "100"
      offsetResetPolicy: latest
      allowIdleConsumers: 'false'
```