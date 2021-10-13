# Habit.Worker

A background worker that subscribes to the event store and write the projection on mongo.

## Features

 - 📑 Projections
 - 🔄 Resubscribe automatically
 - 💖 Health checks

## Configuration

| key                                    | default value |
|----------------------------------------|---------------|
| HealthChecks:FilePath                  | /tmp/healthy  |
| ConnectionStrings:dabit-event-store-db | -             |
| ConnectionStrings:dabit-mongo-db       | -             |

## Health Checks

Health checks are performed through file presence instead of an http endpoint.
If the file exist then the service is healthy, if it doesn't then it's unhealthy.

Both the event store and mongo db connection are monitored.
The default path is `/tmp/healthy`.