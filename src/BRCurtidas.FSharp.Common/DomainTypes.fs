namespace BRCurtidas.FSharp

open System
open System.Text.RegularExpressions
open Chessie.ErrorHandling

type Id = Guid

type EmailAddress = 
    private | EmailAddress of string
    override this.ToString() = match this with EmailAddress x -> x

module EmailAddress =
    type Issue = InvalidEmail

    let create x =
        let regex = Regex("""(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])""")
        if regex.IsMatch x
        then ok (EmailAddress x)
        else fail InvalidEmail
    
    let apply f (EmailAddress x) = f x

    let value x = apply id x

type Url =
    private | Url of string
    override this.ToString() = match this with Url x -> x

module Url =
    type Issue = InvalidUrl

    let create x =
        match Uri.TryCreate(x, UriKind.Absolute) with
        | (true, _) -> ok (Url x)
        | _ -> fail InvalidUrl

    let apply f (Url x) = f x

    let value x = apply id x

[<AutoOpen>]
module Patterns =
    let (|Id|_|) (x : string) : Id option =
        match Guid.TryParse(x) with
        | (true, g) -> Some g
        | _ -> None

    let (|EmailAddress|) (x : EmailAddress) =
        match x with EmailAddress x -> x

    let (|Url|) (x : Url) =
        match x with Url x -> x