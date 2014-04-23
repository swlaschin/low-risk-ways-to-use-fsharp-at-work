module Program

open System
open Fuchu

// Added for Fuchu -- this allows the EXE to be its own test runner
[<EntryPoint>]
let main args = 
    let exitCode = defaultMainThisAssembly args
    
    Console.WriteLine("Press any key")
    Console.ReadLine() |> ignore

    // return the exit code
    exitCode 
