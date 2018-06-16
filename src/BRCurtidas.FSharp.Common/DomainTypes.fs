namespace BRCurtidas.FSharp

open System
open System.Text.RegularExpressions
open Chessie.ErrorHandling
open System.Collections.Generic
open BCrypt.Net
open System.Globalization

type EmailAddress =
    private | EmailAddress of string
    member this.Value = match this with EmailAddress x -> x
    override this.ToString() = this.Value

[<RequireQualifiedAccess>]
module EmailAddress =
    type Issue =
        | Missing
        | InvalidEmail
        override this.ToString() =
            match this with
            | Missing -> "E-mail address is missing."
            | InvalidEmail -> "Email address is not valid."

    let [<Literal>] RegexPattern = """(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])"""

    let create x =
        let canonicalize (x : string) =
            match x with
            | null -> null
            | x -> x.ToLowerInvariant()
        match canonicalize x with
        | null -> fail Missing
        | x ->
            if Regex(RegexPattern).IsMatch x
            then ok (EmailAddress x)
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
        override this.ToString() =
            match this with
            | Missing -> "URL is missing."
            | InvalidScheme -> "URL scheme is invalid. Accepted schemes are http and https."
            | InvalidUrl -> "URL format is not valid."

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

module Comparable =
    type Issue<'a when 'a : comparison> =
        | MustNotBeMoreThan of 'a
        | MustNotBeLessThan of 'a
        | MustNotHaveValues of 'a list
        override this.ToString() =
            match this with
            | MustNotBeMoreThan x -> sprintf "Value must not be longer than %A." x
            | MustNotBeLessThan x -> sprintf "Value must not be less than %A." x
            | MustNotHaveValues x -> sprintf "Value must not equals any of the following values: %A" x

    let create min max invalidValues ctor (x : 'a when 'a : comparison) =
        let validate (x : 'a) =
            List<Issue<'a>>()
            |> tee (fun issues ->
                if x < min then issues.Add(MustNotBeLessThan min)
                if x > max then issues.Add(MustNotBeMoreThan max)
                let containsValues = invalidValues |> Seq.contains x
                if containsValues then issues.Add(MustNotHaveValues invalidValues))
            |> List.ofSeq
        match validate x with
        | [] -> ok (ctor x)
        | x :: xs -> fail x |> mergeMessages xs

type Id =
    private | Id of int64
    member this.Value = match this with Id x -> x
    override this.ToString() = this.Value.ToString()

[<RequireQualifiedAccess>]
module Id =
    let create x = Comparable.create 1L Int64.MaxValue [] Id x

    let apply f (Id x) = f x

    let value = apply id

type Quantity =
    private | Units of int
    member this.Value = match this with | Units x -> x
    override this.ToString() = this.Value.ToString()

[<RequireQualifiedAccess>]
module Quantity =
    let create x = Comparable.create 0 Int32.MaxValue [] Units x

    let apply f (Units x) = f x

    let value = apply id

[<RequireQualifiedAccess>]
module String =
    type Issue =
        | Missing
        | MustNotBeLongerThan of int
        | MustNotHaveChars of char list
        override this.ToString() =
            match this with
            | Missing -> "Text value is missing."
            | MustNotBeLongerThan x -> sprintf "Text value must not have more than %i characters." x
            | MustNotHaveChars x -> sprintf "Text value must not have any of the following characters: %A" x

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
        | InvalidVerifyingDigits
        override this.ToString() =
            match this with
            | Missing -> "CPF is missing."
            | InvalidCpfFormat -> "CPF number is not recognized as a valid CPF."
            | InvalidVerifyingDigits -> "Verifying digits of CPF are not valid."

    let [<Literal>] RegexPattern = """^\d{3}\.\d{3}\.\d{3}-\d{2}$"""

    let create (x : string) =
        let canonicalize (x : string) =
            x.Trim().Replace(".", "").Replace("-", "")
        let verifyingDigitsAreCorrect (x : string) =
            let numbers = x.Substring(0, 9)
            let mapper x = Int32.Parse(x.ToString())
            let folder (sum, i) n = (sum + n * i, i - 1)
            let sum =
                numbers
                |> Seq.map mapper
                |> Seq.fold folder (0, 10)
                |> fst
            let rem x =
                match x with
                | x when x < 2 -> "0"
                | x -> (11 - x).ToString()
            let digit = rem (sum % 11)
            let numbers = numbers + digit
            let sum =
                numbers
                |> Seq.map mapper
                |> Seq.fold folder (0, 11)
                |> fst
            let digit = digit + (rem (sum % 11))
            x.EndsWith(digit)
        if isNull x
        then fail Missing
        else
            if not (Regex(RegexPattern).IsMatch(x))
            then fail InvalidCpfFormat
            else
                let x = canonicalize x
                if not (verifyingDigitsAreCorrect x)
                then fail InvalidVerifyingDigits
                else ok (Cpf x)

type PasswordHashInfo =
    { Settings : string
      Version : string
      WorkFactor : string }

type PasswordHash =
    private | Hash of string * PasswordHashInfo
    member this.Value = match this with Hash (x, _) -> x
    member this.Info = match this with Hash(_, x) -> x
    override this.ToString() = this.Value

[<RequireQualifiedAccess>]
module PasswordHash =
    type Issue =
        | EmptyPassword
        | InvalidHash of string
        override this.ToString() =
            match this with
            | InvalidHash x -> sprintf "Hash is not recognized as a valid hash. An error ocurred while analyzing the hash text: %s" x
            | EmptyPassword -> "Password is empty. Hash can not be generated."

    let private createHashInfo (x : HashInformation) =
        { Settings = x.Settings
          Version = x.Version
          WorkFactor = x.WorkFactor }

    let ofPassword x =
        if isNull x
        then fail EmptyPassword
        else
            let h = BCrypt.HashPassword(x)
            let i = createHashInfo (BCrypt.InterrogateHash(h))
            ok (Hash (h, i))

    let create x =
        try
            let i = createHashInfo (BCrypt.InterrogateHash(x))
            ok (Hash (x, i))
        with
        | e -> fail (InvalidHash e.Message)

    let verify x (Hash (hash, _)) =
        BCrypt.Verify(x, hash)

    let apply f (Hash (h, i)) = f h i

    let value = apply (fun h _ -> h)

    let info = apply (fun _ i -> i)

type PhoneNumber =
    private | PhoneNumber of string
    member this.Value = match this with PhoneNumber x -> x
    override this.ToString() = this.Value

[<RequireQualifiedAccess>]
module PhoneNumber =
    type Issue =
        | Missing
        | InvalidPhone
        override this.ToString() =
            match this with
            | Missing -> "Phone number is missing."
            | InvalidPhone -> "Phone number is not recognized as a valid phone number."

    let [<Literal>] RegexPattern = """^(?:(?:\+|00)?(55)\s?)?(?:\(?([1-9][0-9])\)?\s?)?(?:((?:9\d|[2-9])\d{3})\-?(\d{4}))$"""

    let create x =
        match x with
        | null -> fail Missing
        | x ->
            if Regex(RegexPattern).IsMatch(x)
            then ok (PhoneNumber x)
            else fail InvalidPhone

    let apply f (PhoneNumber x) = f x

    let value = apply id

type Currency =
    private | BRL of decimal
    member this.Value = match this with BRL x -> x
    override this.ToString() = this.Value.ToString("c", CultureInfo.GetCultureInfo("pt-BR"))

[<RequireQualifiedAccess>]
module Currency =
    type Issue =
        | TooManyCents
        | MustBeMoreThanZero
        override this.ToString() =
            match this with
            | TooManyCents -> "There are too many cents on the value. Cents must not have more than two digits."
            | MustBeMoreThanZero -> "Amount must be more than zero."

    let create x =
        let validate x =
            let getDecimalPlaces x =
                let rec helper acc (x : decimal) =
                    if x % 1M <> 0M
                    then helper (acc + 1) (x * 10M)
                    else acc
                helper 0 x
            List<Issue>()
            |> tee (fun issues ->
                if getDecimalPlaces x > 2 then issues.Add(TooManyCents)
                if x < 0M then issues.Add(MustBeMoreThanZero))
            |> List.ofSeq
        match validate x with
        | [] -> ok (BRL x)
        | x :: xs -> fail x |> mergeMessages xs

    let apply f (BRL x) = f x

    let value = apply id