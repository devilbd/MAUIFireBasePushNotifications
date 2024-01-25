1. Needs to get firebase-adminsdk.json and google-services.json by setting up application from firebase console.
2. Copy the files as follow:
   - firebase-adminsdk.json into MAUIPushNotificationsClient\Resources\Raw\
   - firebase-adminsdk.json into PushnotificationsBack root folder
   - google-services.json into MAUIPushNotificationsClient root folder
3. Configure IP and port in app.settings.json from MAUIPushNotificationsClient (root folder)

Remark: PushnotificationsBack is implemented with hardcoded value to listen on any IP on port 9091.

Enjoy!
