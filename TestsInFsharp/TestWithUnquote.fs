module TestWithUnquote
(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Use F# to learn to write unit tests in different styles:
   Unquote examples

Requires Nuget packages: 
* UnQuote

====================================================== *)

open NUnit.Framework
open Swensen.Unquote

[<Test>]
let ``When 2 is added to 2 expect 4``() = 
    test <@ 2 + 2 = 4 @>

[<Test>]
let ``When 2.0 + 2.0 result is 4.0 approximately``() = 
    test 
        <@ 
        let result = 2.0 + 2.0
        result < 4.01 && result > 3.99
        @>

[<Test>]
let ``2 + 2 is 4``() = 
   let result = 2 + 2
   result =? 4

[<Test>]
let ``2 + 2 is bigger than 5``() = 
   let result = 2 + 2
   result >? 5
