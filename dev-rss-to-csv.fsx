(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Convert an RSS feed to CSV

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
#r "System.Xml.Linq.dll"
open FSharp.Data

type Rss = XmlProvider<"https://stackoverflow.com/feeds/tag/f%23">

// prepare a string for writing to CSV            
let prepareStr obj =
    obj.ToString()
     .Replace("\"","\"\"") // replace single with double quotes
     |> sprintf "\"%s\""   // surround with quotes

// convert a list of strings to a CSV
let listToCsv list =
    let combine s1 s2 = s1 + "," + s2
    list 
    |> Seq.map prepareStr 
    |> Seq.reduce combine 

// extract fields from Entry
let extractFields (entry:Rss.Entry) = 
    [entry.Title.Value; 
     entry.Author.Name; entry.Published.ToShortDateString()]

// write the lines to a file
do 
    use writer = new System.IO.StreamWriter("fsharp-questions.csv")
    let feed = Rss.GetSample()
    feed.Entries
    |> Seq.map (extractFields >> listToCsv)
    |> Seq.iter writer.WriteLine
    // writer will be closed automatically at the end of this scope

