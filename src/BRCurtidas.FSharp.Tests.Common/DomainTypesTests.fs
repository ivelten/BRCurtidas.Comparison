module DomainTypesTests

open BRCurtidas.FSharp
open Chessie.ErrorHandling
open FsCheck.Xunit
open System.Text.RegularExpressions
open System.Linq
open System

[<Property>]
let ``EmailAddress should return success when address is valid`` (email : string) =
    let regex = Regex(EmailAddress.regexPattern)
    match EmailAddress.create email with
    | Ok (x, _) -> regex.IsMatch(email) && x.Value = email
    | Bad issues ->
        issues
        |> Seq.forall (fun issue ->
            match issue with
            | EmailAddress.Issue.Missing -> isNull email
            | EmailAddress.Issue.InvalidEmail -> not (regex.IsMatch(email)))

[<Property>]
let ``UserName should return success when user name is valid`` (user : string) =
    match UserName.create user with
    | Ok (x, _) -> x.Value = user.ToLowerInvariant()
    | Bad issues -> 
        issues 
        |> Seq.forall (fun issue ->
            match issue with
            | String.Issue.MustNotHaveChars chars -> chars |> Seq.exists (fun c -> user.Contains(c))
            | String.Issue.MustNotBeLongerThan x -> user.Length > x
            | String.Issue.Missing -> isNull user)

[<Property>]
let ``Url should return success when URL is valid`` (url : string) =
    let (isUri, _) = Uri.TryCreate(url, UriKind.Absolute)
    match Url.create url with
    | Ok (x, _) -> isUri && x.Value = url
    | Bad issues ->
        issues
        |> Seq.forall (fun issue ->
            match issue with
            | Url.Issue.InvalidUrl -> not isUri)