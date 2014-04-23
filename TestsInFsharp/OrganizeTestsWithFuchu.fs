module OrganizeTestsWithFuchu

open Fuchu

// good version of add1
// let add1 x = x + 1

// buggy version of add1, fails on multiples of 9
let add1 x = if (x % 9 <> 0) then x + 1 else x    

// a simple test using any assertion framework:
// Fuchu's own, Nunit, FsUnit, etc
let ``Assert that add1 is x+1`` x _notUsed = 
   NUnit.Framework.Assert.AreEqual(x+1, add1 x)

// a single test case with one value
let simpleTest = 
   testCase "Test with 42" <| 
     ``Assert that add1 is x+1`` 42

// a parameterized test case with one param
let parameterizedTest i = 
   testCase (sprintf "Test with %i" i) <| 
     ``Assert that add1 is x+1`` i

// create a hierarchy of tests 
// mark it as the start point with the "Tests" attribute
[<Fuchu.Tests>]
let tests = 
   testList "Test group A" [
      simpleTest 
      testList "Parameterized 1..10" ([1..10] |> List.map parameterizedTest) 
      testList "Parameterized 11..20" ([11..20] |> List.map parameterizedTest) 
   ]