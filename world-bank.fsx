(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Get data from the World Bank

World Bank Type Provider page: https://fsharp.github.io/FSharp.Data/library/WorldBank.html

You'll probably need an API key if you're playing around a lot!
https://developers.google.com/console/help/?csw=1#activatingapis

See the F# Data Access page for more (http://fsharp.org/data-access/)

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

let data = WorldBankData.GetDataContext()


// =================================
// Compare malnutrition rates
// =================================

// Create a list of countries to process
let groups = 
 [| data.Countries.``Low income``
    data.Countries.``High income``
    |]

// get data from an indicator for particular year
let getYearValue (year:int) (ind:Runtime.WorldBank.Indicator) =
    ind.Name,year,ind.Item year

// get data
[ for c in groups -> 
    c.Name,
    c.Indicators.``Malnutrition prevalence, weight for age (% of children under 5)`` |> getYearValue 2010
] 
// print the data
|> Seq.iter (
    fun (group,(indName, indYear, indValue)) -> 
       printfn "%s -- %s %i %0.2f%% " group indName indYear indValue)

(*
Low income -- Malnutrition prevalence, weight for age (% of children under 5) 2010 23.19% 
High income -- Malnutrition prevalence, weight for age (% of children under 5) 2010 1.36% 
*)

// =================================
// Compare maternal mortality rates
// =================================

// Create a list of countries to process
let countries = 
 [| data.Countries.``European Union``
    data.Countries.``United Kingdom``
    data.Countries.``United States`` |]

// get data
[ for c in countries  -> 
    c.Name,
    c.Indicators.``Maternal mortality ratio (modeled estimate, per 100,000 live births)`` |> getYearValue 2010
] 
// print the data
|> Seq.iter (
    fun (group,(indName, indYear, indValue)) -> 
       printfn "%s -- %s %i %0.2f%% " group indName indYear indValue)

(*
European Union -- Maternal mortality ratio (modeled estimate, per 100,000 live births) 2010 9.00% 
United Kingdom -- Maternal mortality ratio (modeled estimate, per 100,000 live births) 2010 12.00% 
United States -- Maternal mortality ratio (modeled estimate, per 100,000 live births) 2010 21.00% 
*)
