#include <WiFiNINA.h>
#include <LSM6DS3.h>
#include <Wire.h>
#include <avr/dtostrf.h>
#include "arduino_secrets.h"

#define SERVER "192.168.0.178"
#define SERVERPORT 8080

#define TOTAL_WAIT_TIME 60000 // 1 minute
#define ATTEMPT_TIME 5000     // 5 seconds between attempts

// Started from a Copy from https://alistairevans.co.uk/2019/05/27/streaming-real-time-sensor-data-to-an-asp-net-core-app-from-an-arduino/

WiFiClient client;
void setup()
{
  Serial.begin(9600);

  delay(2000);

  unsigned long startTime = millis();
  unsigned long lastAttemptTime = 0;
  int wifiStatus;

  // attempt to connect to Wifi network in a loop,
  // until we connect.
  while (wifiStatus != WL_CONNECTED)
  {
    unsigned long currentTime = millis();

    if (currentTime - startTime > TOTAL_WAIT_TIME)
    {
      // Exceeded the total timeout for trying to connect, so stop.
      Serial.println("Failed to connect");
      while (true)
        ;
    }
    else if (currentTime - lastAttemptTime > ATTEMPT_TIME)
    {
      // Exceeded our attempt delay, initiate again.
      Serial.println("Attempting Wifi Connection");
      lastAttemptTime = currentTime;
      wifiStatus = WiFi.begin(SECRET_SSID, SECRET_PASS);
    }
    else
    {
      // wait 500ms before we check the WiFi status.
      delay(500);
    }
  }

  if (!IMU.begin())
  {
    Serial.println("Failed to initialize IMU!");
    while (1)
      ;
  }

  Serial.println("Connected!");
}

void loop()
{
  Serial.println("Looping");

  int success = 1;
  char request[256];
  char body[64];

  if (!client.connected())
  {
    success = client.connect(SERVER, SERVERPORT);
  }

  Serial.println("AfterConnected");
  Serial.println(success);

  if (success)
  {
    // Empty the buffer
    // (I still don't really care about the response)
    while (client.read() != -1)
      ;

    // Clear the request data
    memset(request, 0, 256);
    // Clear the body data
    memset(body, 0, 64);

    float x, y, z;

    if (IMU.accelerationAvailable())
    {
      IMU.readAcceleration(x, y, z);
    }

    float gx, gy, gz;

    if (IMU.gyroscopeAvailable())
    {
      IMU.readGyroscope(gx, gy, gz);
    }

    sprintf(body, "justSomething=");
    dtostrf(x, 2, 3, &body[strlen(body)]);
    body[strlen(body)] = ',';
    dtostrf(y, 2, 3, &body[strlen(body)]);
    body[strlen(body)] = ',';
    dtostrf(z, 2, 3, &body[strlen(body)]);
    body[strlen(body)] = ',';
    dtostrf(gx, 2, 3, &body[strlen(body)]);
    body[strlen(body)] = ',';
    dtostrf(gy, 2, 3, &body[strlen(body)]);
    body[strlen(body)] = ',';
    dtostrf(gz, 2, 3, &body[strlen(body)]);

    char *currentPos = request;

    // I'm using sprintf for the fixed length strings here
    // to make it easier to read.
    currentPos += sprintf(currentPos, "POST /imu/receiveData HTTP/1.1\r\n");
    currentPos += sprintf(currentPos, "Host: %s:%d\r\n", SERVER, SERVERPORT);
    currentPos += sprintf(currentPos, "Connection: keep-alive\r\n");
    currentPos += sprintf(currentPos, "Content-Length: %d\r\n", strlen(body));
    currentPos += sprintf(currentPos, "Content-Type: application/x-www-form-urlencoded\r\n");
    currentPos += sprintf(currentPos, "\r\n");

    strcpy(currentPos, body);

    // Send the entire request
    client.print(request);

    // Force the wifi module to send the packet now
    // rather than buffering any more data.
    client.flush();
  }
}