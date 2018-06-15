namespace BRCurtidas.FSharp.Data

open BRCurtidas.FSharp
open System

[<CustomEquality; NoComparison>]
type Entity<'T> =
    { Id : Id
      Value : 'T }
    override this.Equals(other) =
        match other with
        | :? Entity<'T> as x -> x.Id = this.Id
        | _ -> false
    override this.GetHashCode() = this.Id.Value.GetHashCode()

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

and SocialNetworkType =
    | Facebook = 1
    | Instagram = 2
    | YouTube = 3

and SocialNetworkAccount =
    { Type : SocialNetworkType
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