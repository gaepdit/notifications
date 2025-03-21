# Notifications API

This application provides an ultra-simple API for publishing notifications.

## Notifications

To add a notification, simply update the `ApiData/Notifications` section of the `appsettings.json` file. Multiple
notifications can be added. The API will update automatically when the app settings file is edited without needing to restart
the application.

```json
{
  "ApiData": {
    "Notifications": [
      {
        "Message": "Add notification message here.",
        "DisplayStart": "2025-01-01T09:00",
        "DisplayEnd": "2025-01-01T21:00"
      }
    ]
  }
}
```

## API Endpoints

There is one API endpoint available:

* **GET** `/current`

  Returns all notifications that are currently active and should be displayed based on the current date and time.
