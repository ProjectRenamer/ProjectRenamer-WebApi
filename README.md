# ProjectRenamer

This template contains
  * Swagger
  * Db Connection (MySql)
  * Db Migration
  * Cors
  * General Exception Handler (filter)
  
  * Dockerfile
  * docker-compose
  
When you are in the project directory, below commands can be executed over Terminal/Powershell:
  * docker-compose up
  * docker-compose scale webapi=3
  
After execution of commands, webapi project raises in three containers. Webapi gives service over localhost:8081... If you request /health-check endpoint, as a response, three different values can be seen successively. Those are the docker container's partial ids.
