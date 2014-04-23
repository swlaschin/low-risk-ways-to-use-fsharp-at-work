(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Use FAKE for build and CI scripts

This is a very simple example of a FAKE script.
For more see https://fsharp.github.io/FAKE/
====================================================== *)

// Include Fake lib
// Assumes NuGet has been used to fetch the FAKE libraries
#r "FAKE/tools/FakeLib.dll"
open Fake

// Properties
let buildDir = "./build/"

// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "Default" (fun _ ->
    trace "Hello World from FAKE"
)

// Dependencies
"Clean"
  ==> "Default"

// start build
RunTargetOrDefault "Default"
