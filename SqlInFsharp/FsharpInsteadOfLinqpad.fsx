(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Use F# instead of LINQpad


 === Setup ===
 1. Install Chocolately from http://chocolatey.org/
 2. Install NuGet command line
    cinst nuget.commandline
 3. Install FSharp.Data.TypeProviders in same directory as script
    nuget install FSharp.Data.TypeProviders -o Packages -ExcludeVersion 

====================================================== *)

// sets the current directory to be same as the script directory
System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

// Requires FSharp.Data under script directory 
//    nuget install FSharp.Data.TypeProviders -o Packages -ExcludeVersion 
#r @"Packages\FSharp.Data.TypeProviders\lib\net40\FSharp.Data.TypeProviders.dll"
#r @"System.Data.Linq.dll"

open System
open System.Linq
open System.Data
open Microsoft.FSharp.Data.TypeProviders
open System.Text.RegularExpressions

[<Literal>]
let connectionString = "Data Source=localhost; Initial Catalog=SqlInFsharp; Integrated Security=True;"

type Sql = SqlDataConnection<connectionString>
let db = Sql.GetDataContext()

// find the number of customers with a gmail domain (in LINQ)
query {
    for c in db.Customer do
    where (c.Email.EndsWith("gmail.com"))
    select c
    count
    }

// optional, turn logging on
// db.DataContext.Log <- Console.Out


// find the number of customers with a gmail domain (in F#)
db.Customer
|> Seq.filter (fun c -> c.Email.EndsWith("gmail.com"))
|> Seq.length



// find the most popular domain for people born in each decade
let getDomain email =
    Regex.Match(email,".*@(.*)").Groups.[1].Value

let getDecade (birthdate:Nullable<DateTime>) =
    if birthdate.HasValue then
        birthdate.Value.Year / 10  * 10 |> Some
    else
        None

let topDomain list = 
    list
    |> Seq.distinct
    |> Seq.head
    |> snd

db.Customer
|> Seq.map (fun c -> getDecade c.Birthdate, getDomain c.Email)
|> Seq.groupBy fst
|> Seq.sortBy fst
|> Seq.map (fun (decade, group) -> (decade,topDomain group))
|> Seq.iter (printfn "%A")


