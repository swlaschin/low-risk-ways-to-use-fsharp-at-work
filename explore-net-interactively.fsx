(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)

(* ======================================================
Use F# to explore the .NET framework interactively
====================================================== *)

// -----------------------------------------------------
// Have I got a custom DateTime format string correct? 
// -----------------------------------------------------
open System
DateTime.Now.ToString("yyyy-MM-dd hh:mm")  // "2014-04-18 01:08"
DateTime.Now.ToString("yyyy-MM-dd HH:mm")  // "2014-04-18 13:09"


// -----------------------------------------------------
// How does XML serialization handle local DateTimes vs UTC DateTimes? 
// -----------------------------------------------------

// TIP: sets the current directory to be same as the script directory
System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

open System

[<CLIMutable>] 
type DateSerTest = {Local:DateTime;Utc:DateTime}

let ser = new System.Xml.Serialization.XmlSerializer(typeof<DateSerTest>)

let testSerialization (dt:DateSerTest) = 
    let filename = "serialization.xml"
    use fs = new IO.FileStream(filename , IO.FileMode.Create)
    ser.Serialize(fs, o=dt)
    fs.Close()
    IO.File.ReadAllText(filename) |> printfn "%s"

let d = { 
    Local = DateTime.SpecifyKind(new DateTime(2014,7,4), DateTimeKind.Local)
    Utc = DateTime.SpecifyKind(new DateTime(2014,7,4), DateTimeKind.Utc)
    }

testSerialization d

(*
// output is => 
<DateSerTest xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Local>2014-07-04T00:00:00+01:00</Local>
  <Utc>2014-07-04T00:00:00Z</Utc>
</DateSerTest>
*)


// -----------------------------------------------------
// Is GetEnvironmentVariable case-sensitive?
// -----------------------------------------------------

Environment.GetEnvironmentVariable "ProgramFiles" = 
    Environment.GetEnvironmentVariable "PROGRAMFILES"
// answer = true => not case-sensitive


