# Light Web Server

This is a very light-weight web server written in C#.

## Features
### Key Features
* Starts up very quickly. (It has VERY few dependencies.)
* Usable in LAN without any special configuration. (It uses `TcpListener` instead of the restrictive `HttpListener`.)
* Very little code. (It may be a good baseline for other more specific projects. I intend to keep it easy to read through all the code in one sitting.)
* Demonstrates serving a small Hello World HTML page, favicons, and can be easily configured to serve all images within a local directory too.
* Theoretically platform-agnostic. (I've only tested on Windows so far.)

### Potential Future Features
* In-memory cache for recent images.
* If you find this app useful, let me know in GitHub discussion area if there are other things it should demonstrate.

### Out Of Scope
* Will not become a robust implementation of HTML web server. (There are likely plenty of weird edge cases that are not handled.)
* Will not be asserting a robust level of security. It is more suitable for self/home usage secnarios. (Assume your own security responsibility for your usage scenarios.)
* Will not be demonstrating how to run this as a Windows Service. (I recommend tech like Topshelf via NuGet for trivializing such upgrades though.)

## Usage
The first time it runs on Windows, it should give you a familiar standard dialog to ask for network permissions.
It will only ask once, and unfortunately Windows doesn't make it very easy to find those settings again afterwards.
These settings are only applicable while the server is running, unlike system-wide firewall port forwarding rules.
* If you want to be able to reach the server from other computers on your network, you may need to accept at least one of the permissions checkboxes.
* If you only want to test it locally from the same machine as you are running the server and know you won't later want to test across the LAN, you can uncheck both the boxes.

## Images
If you change the `Configuration.cs` to point at a valid `BaseImageDir` local directory, then the app will serve all images within that directory and below.
For example if you have the following directory file structure:
```
D:\Images
    \Image1.webp
    \a
        \Image2.jpg
    \b
        \Image3.png
```
Then the app will respond with the contents of those images. For example, on localhost these would be the successful requests:
```
http://localhost:32123/img/Image1.webp
http://localhost:32123/img/a/Image2.jpg
http://localhost:32123/img/b/Image3.png
```
