module TestWithFsUnit
(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Use F# to learn to write unit tests in different styles:
   FsUnit examples

Requires Nuget packages: 
* NUnit 
* FsUnit 

====================================================== *)

open NUnit.Framework
open FsUnit

let inline add x y = x + y

[<Test>]
let ``When 2 is added to 2 expect 4``() = 
    add 2 2 |> should equal 4

[<Test>]
let ``When 2.0 is added to 2.0 expect 4.01``() = 
    add 2.0 2.0 |> should (equalWithin 0.1) 4.01

[<Test>]
let ``When ToLower(), expect lowercase letters``() = 
    "FSHARP".ToLower() |> should startWith "fs"

