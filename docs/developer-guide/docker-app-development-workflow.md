# Docker app development workflow
Use this guide  <a class="crosslink" href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/docker-application-development-process/docker-app-development-workflow" target="_blank">Development workflow for Docker apps</a> to configure how Visual Studio works with docker-compose

Virto Commerce Team created [docker-compose.override.yml](https://github.com/VirtoCommerce/vc-platform/blob/master/docker-compose.override.yml) to run vc-platform and mssql server in containers

> To use it you need to create an external network for the Docker engine

```
docker network create nat
```

To work with the contents of containers, use the mapping folder

```
...
 volumes:
      - ${CMS_CONTENT_VOLUME}:/app/wwwroot/cms-content
      - ${MODULES_VOLUME}:/app/Modules
...
```

> You can parameterize these values ​​in the .env file

```
CMS_CONTENT_VOLUME=/Path/to/folder/cms-content
MODULES_VOLUME=/Path/to/folder/modules
```

If you use Docker Desktop on Windows navigate to <a class="crosslink" href="https://docs.docker.com/docker-for-windows/" target="_blank">Docker for windows</a> for sharing folders