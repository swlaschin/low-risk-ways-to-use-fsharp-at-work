(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)

//==================================
// Simple word counting algorithm
//==================================

open System
open System.IO
open System.Text.RegularExpressions

let files pattern includeSubdirs path = 
    let option = 
        if includeSubdirs 
        then SearchOption.AllDirectories
        else SearchOption.TopDirectoryOnly
    Directory.EnumerateFiles(path, pattern, option)    

let lineWordCount line = 
    Regex.Split(line,@"\b\W+\b").Length // crude

let fileWordCount path = 
    File.ReadAllLines(path)
    |> Array.map lineWordCount 
    |> Array.sum
    |> (fun sum -> (1,sum))

let pathWordCount pattern includeSubdirs path = 
    let add (fileCount1, wordCount1) (fileCount2, wordCount2) =
        fileCount1 + fileCount2, wordCount1 + wordCount2
    
    files pattern includeSubdirs path 
    |> Seq.map fileWordCount 
    |> Seq.reduce add

let path = @"\git_repos\fsharpforfunandprofit.com\_posts"
let pattern = "*.md"
let includeSubdirs = true

pathWordCount pattern includeSubdirs path
||> printfn "File count=%i, word count= %i" 

// if running from command line, wait for input
Console.ReadLine() |> ignore


