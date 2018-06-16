namespace BRCurtidas.FSharp.Data

open System
open BRCurtidas.FSharp
open Chessie.ErrorHandling

[<CustomEquality; NoComparison>]
type Entity<'T> =
    { Id : Id
      Value : 'T }
    override this.Equals(other) =
        match other with
        | :? Entity<'T> as x -> x.Id = this.Id
        | _ -> false
    override this.GetHashCode() = this.Id.Value.GetHashCode()

module Entity =
    let create id value = trial {
        let! id = Id.create id |> stringifyFailure
        return { Id = id; Value = value } }

type Account =
    { UserName : UserName
      PasswordHash : PasswordHash }

and Address =
    { Line1 : String100
      Line2 : String100 option }

and User =
    { Name : String50
      Email : EmailAddress
      Cpf : Cpf
      Phone : PhoneNumber
      Account : Account
      Address : Address }

and PaymentMethod =
    | PayPal = 1
    | PagSeguro = 2
    | WireTransfer = 3

and OrderStatus =
    | Created = 1
    | WaitingPayment = 2
    | Paid = 3
    | Cancelled = 4
    | Delivered = 5

and SocialNetwork =
    | Facebook = 1
    | Instagram = 2
    | YouTube = 3

and SocialNetworkAccount =
    { SocialNetwork : SocialNetwork
      Profile : Url }

and Payment =
    { Reference : string
      Method : PaymentMethod }

and Product =
    { Price : Currency
      Enabled : bool
      Description : String100
      Title : String50 }

and Order =
    { User : User
      Product : Product
      Payment : Payment option
      Status : OrderStatus
      Quantity : Quantity
      Price : Currency
      Created : DateTime
      SocialNetworkAccount : SocialNetworkAccount }

module Account =
    let create userName password = trial {
        let! userName = UserName.create userName |> stringifyFailure
        let! passwordHash = PasswordHash.ofPassword password |> stringifyFailure
        return { UserName = userName; PasswordHash = passwordHash } }

module Address =
    let create line1 line2 = trial {
        let! line1 = String100.create line1 |> stringifyFailure
        match line2 with
        | Some line2 ->
            let! line2 = String100.create line2 |> stringifyFailure
            return { Line1 = line1; Line2 = Some line2 }
        | None -> return { Line1 = line1; Line2 = None } }

module User =
    let create name email cpf phone account address = trial {
        let! name = String50.create name |> stringifyFailure
        let! email = EmailAddress.create email |> stringifyFailure
        let! cpf = Cpf.create cpf |> stringifyFailure
        let! phone = PhoneNumber.create phone |> stringifyFailure
        return { Name = name; Email = email; Cpf = cpf; Phone = phone; Account = account; Address = address } }

module Product =
    let create price enabled description title = trial {
        let! price = Currency.create price |> stringifyFailure
        let! description = String100.create description |> stringifyFailure
        let! title = String50.create title |> stringifyFailure
        return { Price = price; Enabled = enabled; Description = description; Title = title } }

module SocialNetworkAccount =
    let create network profile = trial {
        let! profile = Url.create profile |> stringifyFailure
        return { SocialNetwork = network; Profile = profile } }

module Order =
    let create user (product : Product) payment status qty network = trial {
        let! qty = Quantity.create qty |> stringifyFailure
        let! price =
            (decimal qty.Value) * product.Price.Value
            |> Currency.create
            |> stringifyFailure
        return { User = user
                 Product = product
                 Payment = payment
                 Status = status
                 Quantity = qty
                 Price = price
                 Created = DateTime.Now
                 SocialNetworkAccount = network } }