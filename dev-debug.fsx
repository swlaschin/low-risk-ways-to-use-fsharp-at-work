(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)

open System
open System.Diagnostics

let processList list = 
    let x = 1
    let x = 1
    for elem in list do
        printfn "elem = %i" elem

let add x y =
    x + y 

// if running from command line, wait for input, so you can attach FSI by hand
// printfn "please attach debugger to FSI.EXE"
// Console.ReadLine() |> ignore

// alternatively, launch debugger from inside code
Debugger.Launch()
Debugger.Break()

let list = [1;2;3;4]
processList list

let add1 = add 1
list 
|> List.map add1 
|> printfn "%A"

// if running from command line, wait for input
Console.ReadLine() |> ignore


