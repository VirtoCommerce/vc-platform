ARG platform_arg

FROM docker.pkg.github.com/virtocommerce/vc-deploy-apps/platform-dev:${platform_arg}

WORKDIR /opt/virtocommerce/platform

RUN apt-get update && apt-get install -y openssh-server

COPY wait-for-it.sh /wait-for-it.sh

ENTRYPOINT ["dotnet", "VirtoCommerce.Platform.Web.dll"]
