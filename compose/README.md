# Local dev environment

There is two service that need to be unified under a single endpoint, so the frontend can hit it. You can see
the [docker-compose.yml](docker-compose.yml) for the details.

| :exclamation: | This setup is not intended to be used in production |
|---------------|:----------------------------------------------------|

## Run

Typically you would run these to get a fully working environment + the frontend.

```bash
docker-compose -f compose/docker-compose.yml up -d
dotnet run --project Identity.API
dotnet run --project Habit.API
dotnet run --project Habit.Worker
```

## Endpoints

| name              | endpoint       |
|-------------------|----------------|
| Identity.API      | localhost:5002 |
| Habit.API         | localhost:5001 |
| API main endpoint | localhost:5000 |
| Traefik dashboard | localhost:8080 |

`Identity.API` & `Habit.API` are exposed for convenience reason as this is a dev setup.
In production, only the `API main endpoint` (which is Traefik) would be exposed. This also what the frontend use.