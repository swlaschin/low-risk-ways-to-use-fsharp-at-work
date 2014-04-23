(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Use F# to test your own code interactively
====================================================== *)

// sets the current directory to be same as the script directory
System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

#r @"bin\debug\myapp.dll"
open MyApp

// do something
MyApp.DoSomething()

// before compiling again, be sure to reset the F# interactice session to release the lock on the DLL

