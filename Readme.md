This was an experiment I did to test the HTTPListener to contrast with ASP.Net Core

ASP.Net Core used approximately 115MB of RAM to start, whereas this one started with 9 or 10.
ASP.Net Core climbed up to 300MB before I gave up with flooding it with requests. This experiment stops increasing memory usage at 14MB.

This is a prototype for an ongoing project to ensure that basic web communications are possible without the need for bulkiness. If you use this code, use it merely as a starting point. It is not secure, it does not have authorization, it does not listen to KeepAlive requests, and has not been optimized. This is purely prototype code. 


What's going on here: 
	HttpServer.cs: 
		Upon creation, it starts listening on a hard-coded port (the first one I got to work without fighting - 48435). You can add and remove routes at your whim, but they must adhere to the Request, Response delegate declared at the top of the HttpServer file. Once a route is added, it treats a raw / route as special. All routes saved in the Routes/Delegate dictionary called Routes are lower case, hence why it is perfectly valid to use ERR (all caps) as an error key, since all routes are always lowercase.
		It listens on another thread (thank you, Tasks) for incoming requests, and processes each one at a time by awaiting the execution of the delegate, and passing in the request and response.


My goal to have node-like HTTP access is achieved through this, with significant memory usage drop and stripping the need for ASP.Net Core.