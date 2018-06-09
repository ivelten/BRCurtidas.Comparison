module DomainTypesTests

open Xunit
open BRCurtidas.FSharp
open Chessie.ErrorHandling
open BRCurtidas.FSharp.Tests.Helpers

[<Fact>]
let ``EmailAddress should return success when address is valid`` () =
    let address = "address@host.com"
    match EmailAddress.create address with
    | Ok (EmailAddress x, _) -> x |> equals address
    | _ -> fail "E-mail address is not valid."

[<Fact>]
let ``EmailAddress should return failure when address is not valid`` () =
    match EmailAddress.create "invalidaddress" with
    | Ok _ -> fail "E-mail address should be invalid."
    | _ -> pass
