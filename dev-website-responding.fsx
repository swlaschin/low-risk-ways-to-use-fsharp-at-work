(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Check that a website is responding (e.g. after a deployment)

See the F# community projects page for more (http://fsharp.org/community/projects/)

 === Setup ===
 1. Install Chocolately from http://chocolatey.org/
 2. Install NuGet command line
    cinst nuget.commandline
 3. Install FSharp.Data in same directory as script
    nuget install FSharp.Data -o Packages -ExcludeVersion 

====================================================== *)

// sets the current directory to be same as the script directory
System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

// Requires FSharp.Data under script directory 
//    nuget install FSharp.Data -o Packages -ExcludeVersion  
#r @"Packages\FSharp.Data\lib\net40\FSharp.Data.dll"
open FSharp.Data

let queryServer uri queryParams = 
    try
        let response = Http.Request(uri, query=queryParams, silentHttpErrors = true)
        Some response 
    with
    | :? System.Net.WebException as ex -> None

let sendAlert uri message = 
    // send alert via email, say
    printfn "Error for %s. Message=%O" uri message

let checkServer (uri,queryParams) = 
    match queryServer uri queryParams with
    | Some response -> 
        printfn "Response for %s is %O" uri response.StatusCode 
        if (response.StatusCode <> 200) then
            sendAlert uri response.StatusCode 
    | None -> 
        sendAlert uri "No response"

// test the sites    
let google = "http://google.com", ["q","fsharp"]
let bad = "http://example.bad", []

[google;bad]
|> List.iter checkServer 

    
