(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)

(* ======================================================
Use F# to play with webservices interactively

In this example, we'll create a simple web service using OWIN and WepApi
====================================================== *)

// === Setup ===
// 1. Install Chocolately from http://chocolatey.org/
// 2. Install NuGet command line
//    cinst nuget.commandline
// 3. Install WebApi.OwinSelfHost in same directory as script
//    nuget install Microsoft.AspNet.WebApi.OwinSelfHost -o Packages -ExcludeVersion 

// sets the current directory to be same as the script directory
System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

// assumes nuget install Microsoft.AspNet.WebApi.OwinSelfHost has been run 
// so that assemblies are available under the current directory
#r @"Packages\Owin\lib\net40\Owin.dll"
#r @"Packages\Microsoft.Owin\lib\net40\Microsoft.Owin.dll"
#r @"Packages\Microsoft.Owin.Host.HttpListener\lib\net40\Microsoft.Owin.Host.HttpListener.dll"
#r @"Packages\Microsoft.Owin.Hosting\lib\net40\Microsoft.Owin.Hosting.dll"
#r @"Packages\Microsoft.AspNet.WebApi.Owin\lib\net45\System.Web.Http.Owin.dll"
#r @"Packages\Microsoft.AspNet.WebApi.Core\lib\net45\System.Web.Http.dll"
#r @"Packages\Microsoft.AspNet.WebApi.Client\lib\net45\System.Net.Http.Formatting.dll"
#r @"Packages\Newtonsoft.Json\lib\net40\Newtonsoft.Json.dll"
#r "System.Net.Http.dll"

open System
open Owin 
open Microsoft.Owin
open System.Web.Http 
open System.Web.Http.Dispatcher
open System.Net.Http.Formatting

module OwinSelfhostSample =

    /// a record to return
    [<CLIMutable>]
    type Greeting = { Text : string }

    /// A simple Controller
    type GreetingController() =
        inherit ApiController()

        // GET api/greeting
        member this.Get()  =
            {Text="Hello!"}

    /// Another Controller that parses URIs
    type ValuesController() =
        inherit ApiController()

        // GET api/values 
        member this.Get()  =
            ["value1";"value2"]

        // GET api/values/5 
        member this.Get id = 
            sprintf "id is %i" id 

        // POST api/values 
        member this.Post ([<FromBody>]value:string) = 
            ()

        // PUT api/values/5 
        member this.Put(id:int, [<FromBody>]value:string) =
            ()
        
        // DELETE api/values/5 
        member this.Delete(id:int) =
            () 

    /// A helper class to store routes, etc.
    type ApiRoute = { id : RouteParameter }

    /// IMPORTANT: When running interactively, the controllers will not be found with error:
    /// "No type was found that matches the controller named 'XXX'."
    /// The fix is to override the ControllerResolver to use the current assembly
    type ControllerResolver() =
        inherit DefaultHttpControllerTypeResolver()

        override this.GetControllerTypes (assembliesResolver:IAssembliesResolver) = 
            let t = typeof<System.Web.Http.Controllers.IHttpController>
            System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
            |> Array.filter t.IsAssignableFrom
            :> Collections.Generic.ICollection<Type>    

    /// A class to manage the configuration
    type MyHttpConfiguration() as this =
        inherit HttpConfiguration()

        let configureRoutes() = 
            this.Routes.MapHttpRoute(
                name= "DefaultApi",
                routeTemplate= "api/{controller}/{id}",
                defaults= { id = RouteParameter.Optional }
                ) |> ignore
 
        let configureJsonSerialization() = 
            let jsonSettings = this.Formatters.JsonFormatter.SerializerSettings
            jsonSettings.Formatting <- Newtonsoft.Json.Formatting.Indented
            jsonSettings.ContractResolver <- 
                Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()

        // Here is where the controllers are resolved
        let configureServices() = 
            this.Services.Replace(
                typeof<IHttpControllerTypeResolver>, 
                new ControllerResolver())

        do configureRoutes()
        do configureJsonSerialization()
        do configureServices()

    /// Create a startup class using the configuration    
    type Startup() = 

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        member this.Configuration (appBuilder:IAppBuilder) = 
            // Configure Web API for self-host. 
            let config = new MyHttpConfiguration() 
            appBuilder.UseWebApi(config) |> ignore
    

// Start OWIN host 
do 
    // Create server
    let baseAddress = "http://localhost:9000/" 
    use app = Microsoft.Owin.Hosting.WebApp.Start<OwinSelfhostSample.Startup>(url=baseAddress) 

    // Create client and make some requests to the api
    use client = new System.Net.Http.HttpClient() 

    let showResponse query = 
        let response = client.GetAsync(baseAddress + query).Result 
        Console.WriteLine(response) 
        Console.WriteLine(response.Content.ReadAsStringAsync().Result) 

    showResponse "api/greeting"
    showResponse "api/values"
    showResponse "api/values/42"

    // for standalone scripts, pause so that you can test via your browser as well
    Console.ReadLine() |> ignore

