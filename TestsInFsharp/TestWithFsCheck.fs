module TestWithFsCheck

open NUnit.Framework
open FsCheck
open FsUnit

// ============================================
// test that factorization is correct
// ============================================

module Factorize = 

    let findAFactor num = 
        let factors = [2;3;5;7;11;13;17;19;23;29]
        
        // if it is a factor, return Some and a new sumber
        let testFactor factor = 
            if num % factor = 0 
            then Some factor
            else None

        // pick the first factor, if none is found, 
        // return the original number 
        factors 
        |> Seq.tryPick testFactor 
        |> defaultArg <| num

    let rec factorize' num factorsSoFar = 
        match num with 
        | 1 -> factorsSoFar // we're done
        | _ -> 
            // find a factor
            let factor = findAFactor num
            let newNum = num / factor
            // add the factor to the list and try again
            factorize' newNum (factor::factorsSoFar)

    let factorize num =
        factorize' num [1]

module Factorize_TestWithNunit = 
    open Factorize

    // explicit checking of one value
    [<Test>]
    let ``The factors of 1 are [1]``() = 
        let factors = Factorize.factorize 1
        factors |> should equal [1]

    // explicit checking of one value
    [<Test>]
    let ``The factors of 31 are [31;1]``() = 
        let factors = Factorize.factorize 31
        factors |> should equal [31;1]

    // explicit checking of one value
    [<Test>]
    let ``The product of the factors of 120 should be [5; 3; 2; 2; 2; 1]``() = 
        let factors = Factorize.factorize 120
        factors |> should equal [5; 3; 2; 2; 2; 1]


module Factorize_TestWithFsCheck = 
    open Factorize

    // a property that holds for all numbers between 1 and 400
    let ``The product of the factors should be the original number`` num = 
        let factors = Factorize.factorize num
        let productOfFactors = factors |> List.reduce (*)

        // only for numbers between 1 and 400
        num = productOfFactors

    [<Test>]
    let ``Test that the product of the factors should be the original number``() = 
        let isInRange i = (i >= 1) && (i <= 400)
        let property num = 
            isInRange num ==> lazy (``The product of the factors should be the original number`` num)
        Check.Quick property 



// ============================================
// test that Roman Numerals are correct
// ============================================

module RomanNumerals = 

    let arabicToRoman' arabic = 
           (String.replicate arabic "I")
            .Replace("IIIII","V")
            .Replace("VV","X")
            .Replace("XXXXX","L")
            .Replace("LL","C")
            .Replace("CCCCC","D")
            .Replace("DD","M")
            // optional substitutions
            .Replace("IIII","IV")
            .Replace("VIV","IX")
            .Replace("XXXX","XL")
            .Replace("LXL","XC")
            .Replace("CCCC","CD")
            .Replace("DCD","CM")
 
    let arabicToRoman arabic = 
       if arabic < 0 || arabic > 4000 
       then failwith "Number out of range"
       else arabicToRoman' arabic

    let maxRepetitionProperty ch count (input:string) = 
        let find = String.replicate (count+1) ch
        input.Contains find |> not

    // a property that holds for all roman numerals
    let ``has max rep of one V`` roman = 
        maxRepetitionProperty "V" 1 roman 

    // a property that holds for all roman numerals
    let ``has max rep of three Xs`` roman = 
        maxRepetitionProperty "X" 3 roman 


module RomanNumerals_TestWithNunit = 
    open RomanNumerals

    // explicit checking of one value
    [<Test>]
    let ``Test that 497 is CDXCVII``() = 
        RomanNumerals.arabicToRoman 497 |> should equal "CDXCVII"

module RomanNumerals_TestWithFsCheck = 
    open RomanNumerals

    // We want to limit the input to numbers less than 4000, so we need to add a constraint to the input and
    // then only check the property if the input number is within the correct range. Here's the code to do that:
    let testWithRange f num = 
        // setup a filter
        let romanIsInRange i = (i >= 1) && (i <= 4000)
        // if number is in range then check the property 
        romanIsInRange num ==> lazy (f num)

    [<Test>]
    let ``Test that roman numerals have no more than one V``() = 
        let property num = 
            // convert the number to roman and check the property
            RomanNumerals.arabicToRoman num |> ``has max rep of one V``

        Check.Quick (testWithRange property)

    [<Test>]
    let ``Test that roman numerals have no more than three Xs``() = 
        let property num = 
            // convert the number to roman and check the property
            RomanNumerals.arabicToRoman num |> ``has max rep of three Xs``

        Check.Quick (testWithRange property)

    [<Test>]
    let ``Test that roman numerals have no more than two Xs``() = 
        // a property that does NOT hold for all roman numerals
        let ``has max rep of two Xs`` roman = 
            maxRepetitionProperty "X" 2 roman 

        let property num = 
            // convert the number to roman and check the property
            RomanNumerals.arabicToRoman num |> ``has max rep of two Xs``

        Check.Quick (testWithRange property)

