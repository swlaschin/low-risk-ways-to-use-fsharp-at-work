module TestWithNunit
(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Use F# to write NUnit tests 

Requires Nuget packages: 
* NUnit 
====================================================== *)

open NUnit.Framework

// ============================================
// using traditional "TestFixture" class 
// ============================================

[<TestFixture>]
type TestClass() = 

    [<Test>]
    member this.When2IsAddedTo2Expect4() = 
        Assert.AreEqual(4, 2+2)

// ============================================
// 1) Test code is direct in module, without using a class
// 2) Uses double backticks for the test name
// ============================================
[<Test>]
let ``When 2 is added to 2 expect 4``() = 
    Assert.AreEqual(4, 2+2)


[<Test>]
let ``When 2 is added to 2 expect 5``() = 
    Assert.AreEqual(5, 2+2)



