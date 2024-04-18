Goal: Get as much Live Streamed Data from the PolarH10 Sensor and map it as MIDI Data to be received by a DAW or synthesizer and visualized in a browser.

Current approach:
* Copied some code for bluetooth connection from https://github.com/uwburn/cardia to have a starting point. But that will be refactored and changed as time goes on.
* Port all interesting live data from the Polar SDK from https://github.com/polarofficial/polar-ble-sdk to C#
* Use loopMidi (https://www.tobias-erichsen.de/software/loopmidi.html) as a Middle Man to send MIDI Data to the DAW 

* Use a Blazor WASM Application (https://swharden.com/blog/2021-01-07-blazor-canvas-animated-graphics/) to draw
* Use SignalR to push the data from the client Application to the WASM Application (https://stackoverflow.com/questions/11140164/signalr-console-app-example and https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-7.0)

Color Palette:
#151F23 --background, very dark
#25535D -- a little bit lighter blue
#5C662C -- dark yellow
#DDCF28 -- bright yellow
#C9C947 --brighter yellow

* Borrowed heartbeat animation from https://codepen.io/vsync/pen/QwbZjE by Yair Even Or


* For Arduino:
* * Started with tutorial here: https://dronebotworkshop.com/arduino-nano-33-iot/
* * arduino-cli compile Arduino.ino -b arduino:samd:nano_33_iot -p COM3

Possible Tasks:
* Find other ways to scale the values
* Use average speed to control something
* How to switch between modes?

Checklist:
* Windows Energy Settings
** Monitor does not turn of for at least an hour
** Same for going to sleep
* Click in the screen for the visualization sound


https://rbnrpi.wordpress.com/2017/01/27/sonic-pi-remote-gui-to-control-playstop-and-the-starting-position-within-a-piece/
https://github.com/sonic-pi-net/sonic-pi

TODO: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
' was not in a correct format.The input string '-0POST /imu/receiveData HTTP/1.1

Data from IMU:

x example Data: 0.0009, -0.064, -0.15, 0.18, 0.4, -0.5, 0.67, 0.9, 1.21, 0.9 (straight?)
y example Data: 0.314 (straight?), 0.8, 1.1
z example Data: -0.114, 1.002 (straight?)
gx/gy/gz: ~0, +- 150

https://www.open.edu/openlearn/history-the-arts/music/an-introduction-music-theory/content-section-3.10.2

osc testing: https://github.com/yoggy/sendosc?tab=readme-ov-file

