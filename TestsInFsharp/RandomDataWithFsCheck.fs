module RandomDataWithFsCheck

open System
open FsCheck
open NUnit.Framework

// domain objects
type EmailAddress = EmailAddress of string
type PhoneNumber = PhoneNumber of string
type Customer = {
    name: string
    email: EmailAddress
    phone: PhoneNumber
    birthdate: DateTime
    }

// a list of names to sample
let possibleNames = [
    "Georgianne Stephan"
    "Sharolyn Galban"
    "Beatriz Applewhite"
    "Merissa Cornwall"
    "Kenneth Abdulla"
    "Zora Feliz"
    "Janeen Strunk"
    "Oren Curlee"
    ]

// generate a random name by picking from the list at random
let generateName() = 
    FsCheck.Gen.elements possibleNames 

// generate a random EmailAddress by combining random users and domains
let generateEmail() = 
    let userGen = FsCheck.Gen.elements ["a"; "b"; "c"; "d"; "e"; "f"]
    let domainGen = FsCheck.Gen.elements ["gmail.com"; "example.com"; "outlook.com"]
    let makeEmail u d = sprintf "%s@%s" u d |> EmailAddress
    FsCheck.Gen.map2 makeEmail userGen domainGen 

// generate a random PhoneNumber 
let generatePhone() = 
    let areaGen = FsCheck.Gen.choose(100,999)
    let n1Gen = FsCheck.Gen.choose(1,999)
    let n2Gen = FsCheck.Gen.choose(1,9999)
    let makeNumber area n1 n2 = sprintf "(%03i)%03i-%04i" area n1 n2 |> PhoneNumber
    FsCheck.Gen.map3 makeNumber areaGen n1Gen n2Gen 
    
// generate a random birthdate
let generateDate() = 
    let minDate = DateTime(1920,1,1).ToOADate() |> int
    let maxDate = DateTime(2014,1,1).ToOADate() |> int
    let oaDateGen = FsCheck.Gen.choose(minDate,maxDate)
    let makeDate oaDate = float oaDate |> DateTime.FromOADate 
    FsCheck.Gen.map makeDate oaDateGen

// a function to create a customer
let createCustomer name email phone birthdate =
    {name=name; email=email; phone=phone; birthdate=birthdate}

// use applicatives to create a customer generator
let generateCustomer = 
    createCustomer 
    <!> generateName() 
    <*> generateEmail() 
    <*> generatePhone() 
    <*> generateDate() 

[<Test>]
let printRandomCustomers() =
    let size = 0
    let count = 10
    let data = FsCheck.Gen.sample size count generateCustomer

    // print it
    data |> List.iter (printfn "%A")

(*
 
{name = "Georgianne Stephan";
 email = EmailAddress "d@outlook.com";
 phone = PhoneNumber "(420)330-2080";
 birthdate = 11/02/1976 00:00:00;}

{name = "Sharolyn Galban";
 email = EmailAddress "e@outlook.com";
 phone = PhoneNumber "(579)781-9435";
 birthdate = 01/04/2011 00:00:00;}

{name = "Janeen Strunk";
 email = EmailAddress "b@gmail.com";
 phone = PhoneNumber "(265)405-6619";
 birthdate = 21/07/1955 00:00:00;}

{name = "Oren Curlee";
 email = EmailAddress "d@gmail.com";
 phone = PhoneNumber "(123)661-5806";
 birthdate = 27/02/1991 00:00:00;}

{name = "Sharolyn Galban";
 email = EmailAddress "e@outlook.com";
 phone = PhoneNumber "(847)448-2990";
 birthdate = 15/04/1932 00:00:00;}

{name = "Kenneth Abdulla";
 email = EmailAddress "f@gmail.com";
 phone = PhoneNumber "(868)899-0346";
 birthdate = 23/11/1967 00:00:00;}

{name = "Sharolyn Galban";
 email = EmailAddress "c@example.com";
 phone = PhoneNumber "(554)523-9360";
 birthdate = 11/01/2003 00:00:00;}

{name = "Beatriz Applewhite";
 email = EmailAddress "d@example.com";
 phone = PhoneNumber "(378)942-6716";
 birthdate = 18/08/1944 00:00:00;}

{name = "Oren Curlee";
 email = EmailAddress "a@outlook.com";
 phone = PhoneNumber "(399)566-3900";
 birthdate = 07/10/1979 00:00:00;}

{name = "Georgianne Stephan";
 email = EmailAddress "c@outlook.com";
 phone = PhoneNumber "(257)018-2752";
 birthdate = 14/05/1921 00:00:00;}

*)
