module DomainTypesTests

open BRCurtidas.FSharp
open Chessie.ErrorHandling
open Xunit

[<Theory>]
[<InlineData("Address@host.cOm")>]
[<InlineData("a@b.c")>]
[<InlineData("a1@b.c")>]
[<InlineData("a@b2.c")>]
let ``EmailAddress should be created when address is valid`` (address) =
    match EmailAddress.create address with
    | Ok (x, _) -> Assert.True(x.Value = address.ToLowerInvariant(), sprintf "Created EmailAddress does not have expected value '%s'." address)
    | _ -> failwith <| sprintf "Address '%s' should be valid." address

[<Theory>]
[<InlineData("address")>]
[<InlineData("address@host")>]
[<InlineData("address.com")>]
let ``EmailAddress should not be created when address is invalid`` (address) =
    match EmailAddress.create address with
    | Ok _ -> failwith <| sprintf "Address '%s' should not be valid." address
    | Bad [ EmailAddress.Issue.InvalidEmail ] -> ()
    | _ -> failwith <| sprintf "Unexpected issues when creating invalid e-mail '%s'." address

[<Fact>]
let ``EmailAddress should not be created when address is missing`` () =
    match EmailAddress.create null with
    | Ok _ -> failwith "A null address should not result in an EmailAddress."
    | Bad [ EmailAddress.Issue.Missing ] -> ()
    | _ -> failwith "Unexpected issues when creating null e-mail."

[<Theory>]
[<InlineData("http://address.com")>]
[<InlineData("http://wwW.address.com.bR")>]
let ``Url should be created when URL address is valid`` (url) =
    match Url.create url with
    | Ok (x, _) -> Assert.True(x.Value = url, sprintf "Created Url does not have the expected value '%s'." url)
    | _ -> failwith <| sprintf "Address '%s' should be valid." url

[<Theory>]
[<InlineData("www.address.com")>]
[<InlineData("www.address")>]
[<InlineData("address")>]
let ``Url should not be created when address is invalid`` (url) =
    match Url.create url with
    | Ok _ -> failwith <| sprintf "URL '%s' should not be valid." url
    | Bad [ Url.Issue.InvalidUrl ] -> ()
    | _ -> failwith <| sprintf "Unexpected issues when creating invalid URL '%s'." url

[<Fact>]
let ``Url should not be created with invalid schema`` () =
    let url = "ftp://address.com"
    match Url.create url with
    | Ok _ -> failwith <| sprintf "URL '%s' should not be valid." url
    | Bad [ Url.Issue.InvalidScheme ] -> ()
    | _ -> failwith <| sprintf "Unexpected issues when creating invalid URL '%s'." url

[<Fact>]
let ``Url should not be created when address is missing`` () =
    match Url.create null with
    | Ok _ -> failwith "A null address should not result in a Url."
    | Bad [ Url.Issue.Missing ] -> ()
    | _ -> failwith "Unexpected issues when creating null URL."