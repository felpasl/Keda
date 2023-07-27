# Install monitoring stack

`kubectl create namespace monitoring`

`helm repo add prometheus-community https://prometheus-community.github.io/helm-charts`

`helm install mon prometheus-community/kube-prometheus-stack -n monitoring` 

Enable Metrics and service monitor on kafka stack

```
helm upgrade kafka bitnami/kafka -n kafka   --set zookeeper.enabled=true  --set replicaCount=1 --value kaf

```