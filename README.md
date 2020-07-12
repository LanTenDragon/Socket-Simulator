# Internet of Things Smart Socket & Dashboard

This smart socket project envisions an internet enabled device that sits in between your wall socket and your electrical appliance. With this smart socket device, we can breathe new life into older electrical appliances by granting them intenet connectivity indirectly through the smart socket. 

### Links
- Web Dashboard: https://lantendragon.southeastasia.cloudapp.azure.com
- Desktop socket simulator: https://github.com/LanTenDragon/Socket-Simulator/releases/tag/v1.0.0
- IoT-Dashboard GitHub repository:
- IoT-Backend GitHub repository:

# Socket Simulator
As the focus of my course is software engineering, I do not have the expertise to develop the hardware. I have instead written a socket simulator to simulate the hardware. Below is the user guide for the smart socket system and dashboard. The executable file for this smart socket simulator is available for download in this repository.<br/>
<br/>
# User Guide - Web Dashboard
## Login and Registering
- No email is required.
- Please use a username and password that is between 4 and 20 characters. 
- As this software is a prototype, there is no password reset functionality as of now, so please ensure that you remember your password.
- Once you are registered, a few test sockets and groups are prepared for you.

## Overview Page
- A general overview of the status of all sockets
- Active sockets are sockets that are currently in the ```ON``` state. inactive refers to sockets that are in the ```OFF``` state.
- Unassigned sockets refers to the number of sockets that are not attached to any group. More details in the next section.

## Sockets Controls Page
- This is the page where you can actually turn the sockets on or off. 
- The text in the button shows the current state of the socket.
- Toggling the button would change the state of that socket.
- Sockets are arranged into groups, each group representing an anrea within your home.
- There may be sockets that do not have a group, perhaps because it was recently added, they are categorised as ```unassigned``` or ```ungrouped```
- **Toggle a socket and observe the changes in the socket simulator desktop app**

## Power Usage Page
- This page shows the sum of power usage of each socket in the past 24 hours, sorted from highest to lowest
- If you did not start the socket simulator before entering this page, there may not be any data to display. 
- **Grab the socket simulator, toggle the socket from the web dashboard, start generating power usage data!**<br/>
<br/>

# User Guide - Socket Simulator
## Login
- There is no register function from the socket Simulator. Register from the website instead

## Socket Simluation
- After you have logged in, the sockets are preloaded, but not yet started.
- Click connect to start the sockets.
- Each socket has two states, one physical state and one logical state.
- The two LED bulbs are used to represent the socket's state. 

### Physical State
- Represents the socket state when it is electrically or physically disconnected from the system
- This state covers the following actions and more
- e.g. Power failure, flipping the switch on the wall, blown safety fuse
- These actions are simulated from the ```PHY Switch``` button. 

### Logical State
- Represents the state that is stored within the server.
- This state can only be modified through the web dashboard.
- The logical state in the server and by extension the dashboard is always the correct version.
- The logical state can be modified by the user while the socket is offline.
- When the socket reconnects to the network, the socket will read the latest logical state from the server and assume that state.
