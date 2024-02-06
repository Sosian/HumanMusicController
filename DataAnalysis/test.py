import matplotlib.pyplot as plt
import json

dictionary = json.load(open('C:\\Users\\flori\\Documents\\HumanMusicController\\SensorReceiverServer\\Recordings\\FirstValidJson', 'r'))
xAxis = [i['time'] for i in dictionary]
yAxis = [i['data'].get('x') for i in dictionary]

plt.grid(True)

plt.plot(xAxis,yAxis)
plt.xlabel('time')
plt.ylabel('x')

plt.show()