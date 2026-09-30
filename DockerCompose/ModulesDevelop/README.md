# Modules development via Docker

Runs the published Virto Commerce platform image with SQL Server, Elasticsearch and Redis, so you can build a module on your machine and load it into a running platform without installing any of those services locally.

The `modules` and `app_data/modules` folders on your machine are bind-mounted into the platform container. A module you build and copy there is picked up on the next platform restart.

For the full walkthrough, including how to attach the Visual Studio debugger, see [Modules development via docker](../../docs/developer-guide/modules-development-via-docker.md).

## Prerequisites

* Docker Desktop (Linux containers) or Docker Engine with the Compose v2 plugin (`docker compose`)
* About 4 GB of memory available to Docker

## Quick start

1. Copy this folder to your machine.
2. Create `.env.local` next to `.env` and set at least `MODULES_VOLUME` and `APP_DATA_MODULES` to existing folders on your machine. Leave the shared `.env` unchanged, see [Local overrides](#local-overrides):

   ```dotenv
   MODULES_VOLUME=C:/vc/modules
   APP_DATA_MODULES=C:/vc/app_data/modules
   ```

3. Start the stack. `--wait` returns once every service reports healthy:

   ```bash
   docker compose --env-file .env --env-file .env.local up -d --wait
   ```

4. Open the Platform Manager at http://localhost:8090 and install the modules you need, including the Elasticsearch 8 module if you use search.
5. Check health at http://localhost:8090/health. Every entry should report `Healthy`.

## Configuration

`.env` holds the shared defaults. Variables marked required stop `docker compose` with an error if they are missing.

| Variable | Required | Default | Description |
|---|---|---|---|
| `PLATFORM_VERSION` | | `dev-linux-latest` | Tag of the `ghcr.io/virtocommerce/platform` image |
| `MODULES_VOLUME` | yes | | Host folder mounted to `/opt/virtocommerce/platform/modules` |
| `APP_DATA_MODULES` | yes | | Host folder mounted to `/opt/virtocommerce/platform/app_data/modules` |
| `DB_PASS` | yes | | SQL Server `sa` password. Must meet SQL Server complexity rules |
| `REDIS_PASS` | yes | | Redis password |
| `SEARCH_PROVIDER` | | `ElasticSearch8` | Platform search provider |
| `DOCKER_PLATFORM_PORT` | | `8090` | Host port for the platform |
| `DOCKER_SQL_PORT` | | `1433` | Host port for SQL Server |
| `DOCKER_ELASTIC_PORT` | | `9200` | Host port for Elasticsearch |
| `DOCKER_REDIS_PORT` | | `6379` | Host port for Redis |

## Local overrides

Keep `.env` and `docker-compose.yml` as they are and put your own values in two git-ignored files next to them.

**`.env.local`** holds your values for the variables in the table above. Add only the lines that differ from `.env`:

```dotenv
MODULES_VOLUME=C:/vc/modules
APP_DATA_MODULES=C:/vc/app_data/modules
DB_PASS=My0wn!Passw0rd
DOCKER_ELASTIC_PORT=9201
```

Pass both files on every command. The later file wins, and variables already set in your shell win over both:

```bash
docker compose --env-file .env --env-file .env.local up -d --wait
```

`.env.local` must exist once you pass it, even if it's empty.

To avoid repeating the flags, set `COMPOSE_ENV_FILES=.env,.env.local` in your shell or user environment. Plain `docker compose` commands, including the ones below, then read both files.

**`compose.override.yaml`** holds any other platform setting, such as a connection string, a module setting or a log level. Docker Compose loads it automatically when you run commands from this folder without `-f`, and merges its entries into `docker-compose.yml`. An entry with the same key replaces the shared one:

```yaml
services:
  virtocommerce.platform.web:
    environment:
      Serilog__MinimumLevel__Default: Debug
      ConnectionStrings__VirtoCommerce: "Data Source=host.docker.internal,1433;Initial Catalog=VirtoCommerce3;User ID=sa;Password=My0wn!Passw0rd;TrustServerCertificate=True;"
```

Setting keys use `__` for nesting, so `ConnectionStrings__VirtoCommerce` maps to `ConnectionStrings:VirtoCommerce` in `appsettings.json`.

To see the configuration Compose will actually run, with both files applied:

```bash
docker compose --env-file .env --env-file .env.local config
```

## Services

| Service | Image | Container port |
|---|---|---|
| `virtocommerce.platform.web` | `ghcr.io/virtocommerce/platform:${PLATFORM_VERSION}` | 8080 |
| `vc-db` | `mcr.microsoft.com/mssql/server:2022-latest` | 1433 |
| `elastic` | `docker.elastic.co/elasticsearch/elasticsearch:8.19.4` (security disabled) | 9200 |
| `redis` | `redis:7-alpine` | 6379 |

The platform starts only after SQL Server, Elasticsearch and Redis pass their healthchecks. Database data is kept in the `db-volume` volume between runs.

## Everyday commands

Restart the platform after copying a rebuilt module:

```bash
docker compose restart virtocommerce.platform.web
```

Follow the platform log:

```bash
docker compose logs -f virtocommerce.platform.web
```

Stop the stack and keep the database:

```bash
docker compose down
```

Stop the stack and delete the database:

```bash
docker compose down -v
```

## Troubleshooting

* **`port is already allocated`**: another process uses one of the host ports. Set the matching `DOCKER_*_PORT` variable in `.env` to a free port.
* **`vc-db` never becomes healthy**: `DB_PASS` probably fails SQL Server password rules (at least 8 characters, with three of: upper case, lower case, digits, symbols). Check `docker compose logs vc-db`.
* **First request is slow or times out**: the platform warms up on the first request after starting or after a long idle period. Retry after a few seconds.
* **Module is not loaded**: make sure the module folder is directly under `MODULES_VOLUME` and contains `module.manifest`, then restart the platform and check its log for `Discovered modules`.
