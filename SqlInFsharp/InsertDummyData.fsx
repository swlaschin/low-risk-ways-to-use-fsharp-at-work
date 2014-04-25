(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)

(* ======================================================
Use FsCheck to create dummy records

 === Setup ===
 1. Install Chocolately from http://chocolatey.org/
 2. Install NuGet command line
    cinst nuget.commandline
 3. Install FsCheck in same directory as script
    nuget install FsCheck -o Packages -ExcludeVersion 
 4. Install FSharp.Data.TypeProviders in same directory as script
    nuget install FSharp.Data.TypeProviders -o Packages -ExcludeVersion 

====================================================== *)

// sets the current directory to be same as the script directory
System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

// Requires FsCheck under script directory 
//    nuget install FsCheck -o Packages -ExcludeVersion 
#r @"Packages\FsCheck\lib\net40-Client\FsCheck.dll"

// Requires FSharp.Data.TypeProviders under script directory 
//    nuget install FSharp.Data.TypeProviders -o Packages -ExcludeVersion 
#r @"Packages\FSharp.Data.TypeProviders\lib\net40\FSharp.Data.TypeProviders.dll"
#r @"System.Data.Linq.dll"

open System
open System.Linq
open System.Data
open Microsoft.FSharp.Data.TypeProviders
open FsCheck

[<Literal>]
let connectionString = "Data Source=localhost; Initial Catalog=SqlInFsharp; Integrated Security=True;"

type Sql = SqlDataConnection<connectionString>


// a list of names to sample
let possibleFirstNames = 
    ["Merissa";"Kenneth";"Zora";"Oren"]
let possibleLastNames = 
    ["Applewhite";"Feliz";"Abdulla";"Strunk"]

// generate a random name by picking from the list at random
let generateFirstName() = 
    FsCheck.Gen.elements possibleFirstNames 

let generateLastName() = 
    FsCheck.Gen.elements possibleLastNames

// generate a random email address by combining random users and domains
let generateEmail() = 
    let userGen = FsCheck.Gen.elements ["a"; "b"; "c"; "d"; "e"; "f"]
    let domainGen = FsCheck.Gen.elements ["gmail.com"; "example.com"; "outlook.com"]
    let makeEmail u d = sprintf "%s@%s" u d 
    FsCheck.Gen.map2 makeEmail userGen domainGen 
  
// Generate a random nullable age.
// Note that because age is nullable, 
// the compiler forces us to take that into account
let generateAge() = 
    let nonNullAgeGenerator = 
        FsCheck.Gen.choose(1,99) 
        |> FsCheck.Gen.map (fun age -> Nullable age)
    let nullAgeGenerator = 
        FsCheck.Gen.constant (Nullable())

    // 19 out of 20 times choose a non null age
    FsCheck.Gen.frequency [ 
        (19,nonNullAgeGenerator) 
        (1,nullAgeGenerator)
        ]

// a function to create a customer
let createCustomerImport first last email age =
    let c = new Sql.ServiceTypes.CustomerImport()
    c.FirstName <- first
    c.LastName <- last
    c.EmailAddress <- email
    c.Age <- age
    c //return new record

// use applicatives to create a customer generator
let generateCustomerImport = 
    createCustomerImport 
    <!> generateFirstName() 
    <*> generateLastName() 
    <*> generateEmail() 
    <*> generateAge() 


let insertAll() =
    use db = Sql.GetDataContext()

    // optional, turn logging on or off
    // db.DataContext.Log <- Console.Out
    // db.DataContext.Log <- null

    let insertOne counter customer =
        db.CustomerImport.InsertOnSubmit customer
        // do in batches of 1000
        if counter % 1000 = 0 then
            db.DataContext.SubmitChanges()

    // generate the records
    let count = 10000
    let generator = FsCheck.Gen.sample 0 count generateCustomerImport

    // insert the records
    generator |> List.iteri insertOne
    db.DataContext.SubmitChanges() // commit any remaining

// do it and time it
#time
insertAll() 
#time

