# weather-api-wrapper
to run the project firstly clone the repo 
and to run locally hardcode the api key into appsettings.json
then in your terminal
docker build -t weather-api-wrapper .
docker run -p 8080:8080 weather-api-wrapper
then go to a browser and http://localhost:8080/weather?city=London or whatever city


for the azure container registry method
first set the secrets in your repo settings
head on over to actions and run the pipeline
this should send the container to ACR

To pull and run the image from ACR
docker pull acrcrg.azurecr.io/aspcoresample:<image_id_from_acr>
then to run the image
docker run -p 8080:8080 -e WEATHER_API_KEY=your_key acrcrg.azurecr.io/aspcoresample:1b214a811629ad5572caa8790bec50e805b660b4
Then as above head on over to a local browser and so and so
