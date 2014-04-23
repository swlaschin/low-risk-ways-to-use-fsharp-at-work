(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Use F# to run unit tests without having to install NUnit

From command line run:
  bin\debug\TestsInFsharp.exe
====================================================== *)

System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
System.Diagnostics.Process.Start(__SOURCE_DIRECTORY__ + @"\bin\debug\TestsInFsharp.exe");