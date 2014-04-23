(*
From the article "Low risk ways to use F# at work"
http://fsharpforfunandprofit.com/posts/low-risk-ways-to-use-fsharp-at-work/
*)


(* ======================================================
Use F# to play with UI's interactively

In this example, we'll create a simple WinForms app and modify it in real time.
====================================================== *)

// Step 1 - initial form
open System
open System.Windows.Forms 
open System.Drawing

let form = new Form(Width= 400, Height = 300, Visible = true, Text = "Hello World") 
form.TopMost <- true
form.Click.Add (fun _ -> 
    form.Text <- sprintf "form clicked at %i" DateTime.Now.Ticks)
form.Show()

// Step 2 - add a button
let panel = new FlowLayoutPanel()
form.Controls.Add(panel)
panel.Dock = DockStyle.Fill 
panel.WrapContents <- false 

let greenButton = new Button()
greenButton.Text <- "Make the background color green" 
greenButton.Click.Add (fun _-> form.BackColor <- Color.LightGreen)
panel.Controls.Add(greenButton) 

// Step 3 - change the size of the button
greenButton.AutoSize <- true

// Step 4 - add another button
let yellowButton = new Button()
yellowButton.Text <- "Make me yellow" 
yellowButton.AutoSize <- true
yellowButton.Click.Add (fun _-> form.BackColor <- Color.Yellow)
panel.Controls.Add(yellowButton) 

// Step 5 - change the flow direction
panel.FlowDirection <- FlowDirection.TopDown

// Step 6 - change the size of the yellow button
yellowButton.Dock <- DockStyle.Fill

