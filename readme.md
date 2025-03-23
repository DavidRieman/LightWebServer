# Light Web Server

This is a very light-weight web server written in C#.

## Features
### Key Features
* Starts up very quickly. (It has VERY few dependencies.)
* Usable in local network without any special firewall configuration. (It uses TcpListener instead of the restrictive HttpListener.)
* Very little code. (It may be a good baseline for other more specific projects.)
* Serves a Hello World page and can be easily configured to serve images from a local directory.
* Theoretically platform-agnostic. (I've only tested on Windows so far.)

### Potential future features:
* Serve a favicon.ico by default.
* In-memory cache for recent images.
* If you find this app useful, let me know in GitHub discussion area if there are other things it should demonstrate.

## Usage
The first time it runs on Windows, it should give you a familiar standard dialog to ask for network permissions.
It will only ask once, and unfortunately Windows doesn't make it very easy to find those settings again afterwards.
These settings are only applicable while the server is running, unlike system-wide firewall port forwarding rules.
* If you want to be able to reach the server from other computers on your network, you may need to accept at least one of the permissions checkboxes.
* If you only want to test it locally from the same machine as you are running the server, you can uncheck all the boxes.
