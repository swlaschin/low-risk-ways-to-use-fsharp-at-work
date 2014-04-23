(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)

(* ======================================================
 Use WMI to get the stats for a process

(e.g. can be useful during and after a load test)

See the F# community projects page for more (http://fsharp.org/community/projects/)

 === Setup ===
 1. Install Chocolately from http://chocolatey.org/
 2. Install NuGet command line
    cinst nuget.commandline
 3. Install FSharp.Management in same directory as script
    nuget install FSharp.Management -o Packages -ExcludeVersion 

====================================================== *)

// sets the current directory to be same as the script directory
System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

// Requires FSharp.Management under script directory 
//    nuget install FSharp.Management -o Packages -ExcludeVersion 
#r @"System.Management.dll"
#r @"Packages\FSharp.Management\lib\net40\FSharp.Management.dll"
#r @"Packages\FSharp.Management\lib\net40\FSharp.Management.WMI.dll"

open FSharp.Management

// get data for the local machine
type Local = WmiProvider<"localhost">
let data = Local.GetDataContext()

// get the time and timezone on the machine
let time = data.Win32_UTCTime |> Seq.head
let tz = data.Win32_TimeZone |> Seq.head
printfn "Time=%O-%O-%O %O:%O:%O" time.Year time.Month time.Day time.Hour time.Minute time.Second 
printfn "Timezone=%O" tz.StandardName 

// find the "explorer" process
let explorerProc = 
    data.Win32_PerfFormattedData_PerfProc_Process
    |> Seq.find (fun proc -> proc.Name.Contains("explorer") )

// get stats about it
printfn "ElapsedTime=%O" explorerProc.ElapsedTime
printfn "ThreadCount=%O" explorerProc.ThreadCount
printfn "HandleCount=%O" explorerProc.HandleCount
printfn "WorkingSetPeak=%O" explorerProc.WorkingSetPeak
printfn "PageFileBytesPeak=%O" explorerProc.PageFileBytesPeak



