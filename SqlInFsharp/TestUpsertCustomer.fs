module TestUpsertCustomer

open System
open System.Linq
open System.Data
open System.Data.Linq
open Microsoft.FSharp.Data.TypeProviders
open NUnit.Framework

// ==============================
// Tests
// ==============================


[<Test>]
let ``When upsert customer called with null id, expect customer created with new id``() = 
    DbLib.resetDatabase() 
    use db = DbLib.Sql.GetDataContext()

    // create customer
    let newId = db.Up_Customer_Upsert(Nullable(),"Alice","x@example.com",Nullable()) 

    // check new id 
    Assert.Greater(newId,0)

    // check one customer exists
    let customerCount = db.Customer |> Seq.length
    Assert.AreEqual(1,customerCount)


[<Test>]
let ``When upsert customer called with existing id, expect customer updated``() = 
    DbLib.resetDatabase() 
    use db = DbLib.Sql.GetDataContext()

    // create customer
    let custId = db.Up_Customer_Upsert(Nullable(),"Alice","x@example.com",Nullable()) 
    
    // update customer
    let newId = db.Up_Customer_Upsert(Nullable custId,"Bob","y@example.com",Nullable()) 
    
    // check id hasnt changed
    Assert.AreEqual(custId,newId)

    // check still only one customer
    let customerCount = db.Customer |> Seq.length
    Assert.AreEqual(1,customerCount)

    // check customer columns are updated
    let customer = db.Customer |> Seq.head
    Assert.AreEqual("Bob",customer.Name)


[<Test>]
let ``When upsert customer called with blank name, expect validation error``() = 
    DbLib.resetDatabase() 
    use db = DbLib.Sql.GetDataContext()

    try
        // try to create customer will a blank name
        db.Up_Customer_Upsert(Nullable(),"","x@example.com",Nullable()) |> ignore
        Assert.Fail("expecting a SqlException")
    with
    | :? System.Data.SqlClient.SqlException as ex ->
        Assert.That(ex.Message,Is.StringContaining("@Name"))
        Assert.That(ex.Message,Is.StringContaining("blank"))


