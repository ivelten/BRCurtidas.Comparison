namespace BRCurtidas.FSharp

open System
open System.Text.RegularExpressions
open Chessie.ErrorHandling
open System.Collections.Generic

type EmailAddress = 
    private | EmailAddress of string
    member this.Value = match this with EmailAddress x -> x
    override this.ToString() = this.Value

[<RequireQualifiedAccess>]
module EmailAddress =
    type Issue = 
        | Missing
        | InvalidEmail

    let [<Literal>] regexPattern = """(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])"""

    let create x =
        let canonicalize (x : string) =
            match x with
            | null -> null
            | x -> x.ToLowerInvariant()
        match canonicalize x with
        | null -> fail Missing
        | x ->
            if Regex(regexPattern).IsMatch x
            then ok (EmailAddress (x.ToLowerInvariant()))
            else fail InvalidEmail
    
    let apply f (EmailAddress x) = f x

    let value = apply id

type Url =
    private | Url of string
    member this.Value = match this with Url x -> x
    override this.ToString() = this.Value

[<RequireQualifiedAccess>]
module Url =
    type Issue = 
        | Missing
        | InvalidScheme
        | InvalidUrl

    let create x =
        match x with
        | null -> fail Missing
        | x ->
            match Uri.TryCreate(x, UriKind.Absolute) with
            | (true, uri) ->
                match uri.Scheme.ToLowerInvariant() with
                | "http" | "https" -> ok (Url x)
                | _ -> fail InvalidScheme
            | _ -> fail InvalidUrl

    let apply f (Url x) = f x

    let value = apply id

[<RequireQualifiedAccess>]
module String =
    type Issue =
        | Missing
        | MustNotBeLongerThan of int
        | MustNotHaveChars of char list

    let map f = 
        function
        | null -> null
        | x -> f x : string

    let create (canonicalize : string -> string) length (invalidChars : char list) ctor (x : string) =
        let validate (x : string) =
            List<Issue>()
            |> tee (fun issues ->
                if x.Length > length then issues.Add(MustNotBeLongerThan length)
                let containsChars = invalidChars |> Seq.exists (fun c -> x |> Seq.contains c)
                if containsChars then issues.Add(MustNotHaveChars invalidChars))
            |> List.ofSeq
        let x = canonicalize x
        match x with
        | null -> fail Missing
        | x ->
            match validate x with
            | [] -> ok (ctor x)
            | x :: xs -> fail x |> mergeMessages xs

type UserName =
    private | UserName of string
    member this.Value = match this with UserName x -> x
    override this.ToString() = this.Value

[<RequireQualifiedAccess>]
module UserName =
    let create x =
        let invalidChars = [ ' ' ]
        let canonicalize (x : string) = 
            match x with
            | null -> null
            | x -> x.ToLowerInvariant()
        String.create canonicalize 20 invalidChars UserName x

    let apply f (UserName x) = f x

    let value = apply id

type String25 =
    private | String25 of string
    member this.Value = match this with String25 x -> x
    override this.ToString() = this.Value

[<RequireQualifiedAccess>]
module String25 =
    let create x = String.create id 25 [] String25 x

    let apply f (String25 x) = f x

    let value = apply id

type String50 =
    private | String50 of string
    member this.Value = match this with String50 x -> x
    override this.ToString() = this.Value

[<RequireQualifiedAccess>]
module String50 =
    let create x = String.create id 50 [] String50 x

    let apply f (String50 x) = f x

    let value = apply id

type String100 =
    private | String100 of string
    member this.Value = match this with String100 x -> x
    override this.ToString() = this.Value

[<RequireQualifiedAccess>]
module String100 =
    let create x = String.create id 100 [] String100 x

    let apply f (String100 x) = f x

    let value = apply id

type Cpf =
    private | Cpf of string
    member this.Value = match this with Cpf x -> x
    override this.ToString() = this.Value

[<RequireQualifiedAccess>]
module Cpf =
    type Issue =
        | Missing
        | InvalidCpfFormat
        | AllDigitsMustBeNumbers
        | InvalidVerifyingDigits

    let [<Literal>] regexPattern = """/^\d{3}\.\d{3}\.\d{3}\-\d{2}$/"""

    let create (x : string) =
        let canonicalize (x : string) = 
            x.Trim().Replace(".", "").Replace("-", "")
        let allDigitsAreNumbers (x : string) =
            x |> Seq.forall (fun c -> Char.IsNumber(c))
        let verifyingDigitsAreCorrect (x : string) =
            let numbers = x.Substring(0, 9)
            let folder (sum, i) n =
                let n = int (Char.GetNumericValue(n))
                (sum + n * i, i - 1)
            let sum = 
                numbers
                |> Seq.fold folder (0, 10) 
                |> fst 
                |> (%) 11
            let rem x =
                match x with
                | x when x < 2 -> "0"
                | x -> (11 - x).ToString()
            let digit = rem sum
            let numbers = numbers + digit
            let sum =
                numbers
                |> Seq.fold folder (0, 11)
                |> fst
                |> (%) 11
            let digit = digit + (rem sum)
            x.EndsWith(digit)
        if isNull x
        then fail Missing
        else
            if not (Regex(regexPattern).IsMatch(x))
            then fail InvalidCpfFormat
            else
                let x = canonicalize x
                if not (allDigitsAreNumbers x)
                then fail AllDigitsMustBeNumbers
                elif not (verifyingDigitsAreCorrect x)
                then fail InvalidVerifyingDigits
                else ok (Cpf x)
                
                
