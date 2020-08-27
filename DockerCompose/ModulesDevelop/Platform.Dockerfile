ARG platform_arg

FROM docker.pkg.github.com/virtocommerce/vc-platform/platform:${platform_arg}

WORKDIR /opt/virtocommerce/platform

RUN apt-get update && apt-get install -y openssh-server dos2unix
COPY wait-for-it.sh /wait-for-it.sh
RUN dos2unix /wait-for-it.sh && chmod +x /wait-for-it.sh

ENTRYPOINT ["dotnet", "VirtoCommerce.Platform.Web.dll"]
