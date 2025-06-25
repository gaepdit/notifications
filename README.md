# Notifications API

This application provides an ultra-simple API for publishing notifications to be displayed on various web applications.

## Notifications

A notification has the following properties:

```
(string) Message
(DateTime) DisplayStart
(DateTime) DisplayEnd
(bool) Active
```

The `DisplayStart`, `DisplayEnd`, and `Active` properties control when and if the notification should be displayed.

## Public API Endpoints

### GET `/health`

Returns OK if the API is running.

### GET `/current`

Returns all notifications that are currently active and should be displayed based on the current date and time.

### GET `/future`

Returns all notifications that are scheduled to be displayed in the future.

## Restricted API Endpoints

The following endpoints are for administrators and require a valid API key (passed in using the `X-API-Key` header).

Valid API keys are configured in the `appsettings.json` file:

```json
{
  "ApiKeys": [
    "client-api-key-1",
    "client-api-key-2"
  ]
}
```

### GET `/all`

Returns all notifications, including those that have been deactivated or are expired.

### POST `/add`

Creates a new notification. The request body must contain these fields:

```json
{
  "Message": "Notification message",
  "DisplayStart": "2025-06-25T08:00",
  "DisplayEnd": "2050-08-01T21:00"
}
```

### POST `/deactivate`

Deactivates a specified notification. The request body contains only the notification ID:

```json
{
  "Id": "00000000-0000-0000-0000-000000000001"
}
```
