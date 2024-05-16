Setup and Run Instructions:

1. Clone this code to your local machine from git
2. This application is built using .net 8. Downloads can be retrieved from https://dotnet.microsoft.com/en-us/download/dotnet/8.0.
	2.a To run the solution using Visual Studio and etc make sure .net 8 SDK is installed on your machine.
		This can be done by update in the Visual Studio Installer app for windows or you could download the sdk.
	2.b If you simply want to run the application you can either install the SDK and the .net Runtime or just the Runtime
	2.c You will also need to make sure node.js is installed on your machine
3. If you want to run the application there is a script to help do that in the root folder. Run this using the command line
	3.a Before running is not using visual studio run the dotnet build command to make sure dependencies are fetch and there are no machine configuration 
	issues that cause build errors like node not being installed and etc.
	3.b For windows click on the "windows run app.cmd" to launch the site.
4. If you ran the application by using visual studio it will launch a browser window with the url: https://localhost:7005/swagger/index.html.
	4.a The https://localhost:7005/swagger/index.html url is the swagger url for the api. 
		To go to the web app navigate to https://localhost:7005 in your browser and it will redirect to the url.
		Otherwise the web application url is https://localhost:4200.
	4.b If you used one of the scripts there be two command windows open.
		The first will be for the main application itself and should mention listening on a localhost port.
		The second window will be for the ng serve which should state that watch mode enabled, Local: https://127.0.0.1:4200/
		If both of those look good then you can open the url https://localhost:4200/ in your browser of choice


Application Structure and Info:

The application structure for this project is simple and is in two parts. There is the contacts.web.client project which has the scripts and angular ts files.
Then there is the ContactsWeb.Server application which is the .net 8 web api. This is a restful api with a singular controller.
Based on the instructions this seemed to be the best way to create the app. Within the .net application there is the ContactDs.json which is there data will be stored.
The json file gets copied over to the build/publish location and is what the application will be pointed to for data actions.
The front end code calls to the api endpoints which will execute functions in the ContactsDsManager class. I decided to use a mix of angular formcontrol validators
and data annotations to make sure fields were required and in a certain format. The back end handles basic CRUD actions to the json file and on launch ensure the json file is present.
There is a global error handler in the .net app program.cs which looks like:

app.Use(async (context, next) =>
    List<string> blackListedMethods = ["OPTIONS", "PATCH", "TRACE"];
    if (blackListedMethods.Contains(context.Request.Method))
    {
        context.Response.StatusCode = 405;
        return;
    }
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        Utilities.LogError(ex);
        context.Response.Clear();
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(ex.Message);

    }


This can normally be done by adding middleware into the builder services but I like doing it there since it's multi-use and this is where you would generally add in headers for the pipeline.
The front end will catch these errors which would only have a simple exception message and show them to the user in the form of a sweetalert.

There is also error/info logging happening for the application using NLog, this is stored in a log file where the build assembly is located.


Performance and Scalability:

Performance wise the appllication as-is is good. A user wouldn't notice performance slows till the json file got truly large around the 100mb+ in file size. 
The data model is small so it would take an estimated 1000+ records to get to that point. Reading and writing json to a file is not resource intensive so that isn't a major concern.
If the application was deployed to a web server and had multiple concurrent users I would expect to see issues with concurrency and file access issues since it is pointing to a singular file.
To be propery scalable, handle concurrancy, and to handle redundancy having the data store be in a file is not ideal. I would switch over to storing/managing data in a database server and have the application code use ef core.
Options for a database would be traditional SQL Server, Postgres, or NoSQL database like Cassandra. NoSQL for this action would make more sense unless there already having existing sql server instances are available.
There isn't any relationality for this data so NoSQL would make sense and could be used to avoid sql server licensing costs but ultimatly this is a team/business decision.
