# Notifications API

This application provides an ultra-simple API for publishing notifications.

## Notifications

To add a notification, simply update the `ApiData/Notifications` section of the `appsettings.json` file. Multiple
notifications can be added. The API will update automatically when the file is edited without needing to restart
the application.

```json
{
  "ApiData": {
    "Notifications": [
      {
        "Message": "Add your message here.",
        "DisplayStart": "2035-01-01T09:00",
        "DisplayEnd": "2035-01-01T21:00"
      }
    ]
  }
}
```

## API Endpoints

There are two API endpoints available:

### Get all currently active notifications

**GET** `/`

Returns all notifications that are currently active and should be displayed based on the current date and time.

### Get all current and upcoming notifications

**GET** `/upcoming`

Returns all notifications that are currently active or will be active in the future.
