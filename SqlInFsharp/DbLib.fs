module DbLib

// ==========================================================
// The connection string and other common code for database access
// ==========================================================

open System
open System.Linq
open System.Data
open Microsoft.FSharp.Data.TypeProviders

// ==============================
// Setup
// ==============================
[<Literal>]
let connectionString = "Data Source=localhost; Initial Catalog=SqlInFsharp; Integrated Security=True;"

type Sql = SqlDataConnection<connectionString>

// alias the DbContext type
type DbContext = Sql.ServiceTypes.SimpleDataContextTypes.SqlInFsharp

// ==============================
// Utilities
// ==============================


let removeExistingData (db:DbContext) = 
    let truncateTable name = 
        sprintf "TRUNCATE TABLE %s" name
        |> db.DataContext.ExecuteCommand 
        |> ignore

    ["Customer"; "CustomerImport"; "Country"]
    |> List.iter truncateTable

let insertReferenceData (db:DbContext) = 
    [ "US","United States";
      "GB","United Kingdom" ]
    |> List.iter (fun (code,name) -> 
        let c = new Sql.ServiceTypes.Country()
        c.IsoCode <- code;  c.CountryName <- name
        db.Country.InsertOnSubmit c
        )
    db.DataContext.SubmitChanges()

// removes all data and restores db to known starting point
let resetDatabase() =
    use db = Sql.GetDataContext()
    removeExistingData db
    insertReferenceData db
