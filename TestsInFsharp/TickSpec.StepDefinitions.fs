module TickSpec.StepDefinitions

open TickSpec
open NUnit.Framework

type StockItem = { Count : int }

let mutable stockItem = { Count = 0 }

let [<Given>] ``a customer buys a black jumper`` () = 
    ()
      
let [<Given>] ``I have (.*) black jumpers left in stock`` (n:int) =  
    stockItem <- { stockItem with Count = n }
      
let [<When>] ``he returns the jumper for a refund`` () =  
    stockItem <- { stockItem with Count = stockItem.Count + 1 }
      
let [<Then>] ``I should have (.*) black jumpers in stock`` (n:int) =     
    let passed = (stockItem.Count = n)
    Assert.True(passed)