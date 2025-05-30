# weather-api-wrapper

- to run the project firstly clone the repo
- and to run locally hardcode the api key into appsettings.json
- then in your terminal
- docker build -t weather-api-wrapper .
- docker run -p 8080:8080 weather-api-wrapper
- then go to a browser and http://localhost:8080/weather?city=London or whatever city

### for the azure container registry method

- first set the secrets in your repo settings
- head on over to actions and run the pipeline
- this should send the container to ACR

### To pull and run the image from ACR

- docker pull acrcrg.azurecr.io/aspcoresample:<image_id_from_acr>
- then to run the image
- docker run -p 8080:8080 -e WEATHER_API_KEY=your_key acrcrg.azurecr.io/aspcoresample:1b214a811629ad5572caa8790bec50e805b660b4
- Then as above head on over to a local browser and so and so

## Now to test out the deployment via aks

- usually youd do this with admin permissions and run

```bash
az ad sp create-for-rbac
```

- save the json output as a github secret AZURE_CREDENTIALS

#### My account sadly does not have access so I couldnt create the pipeline rather had to deploy manually using kubectl and deployment.yml

```bash
az login
az aks get-credentials --resource-group crg --name akscrg
kubectl get pods
kubectl create secret generic weather-api-secret --from-literal=WEATHER_API_KEY=ENTER_YOUR_ACTUAL_KEY_HERE
kubectl apply -f deployment.yaml
kubectl get svc
```

if youd prefer forwarding it locallly and testing it

```bash
kubectl port-forward svc/weather-api-service 8080:80
```

got to the public ip and forward the query as before
