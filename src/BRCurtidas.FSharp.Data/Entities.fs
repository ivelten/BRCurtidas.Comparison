namespace BRCurtidas.FSharp.Data

open BRCurtidas.FSharp

type Account =
    { Id : int64
      UserName : UserName
      PasswordHash : string }

and Address =
    { Id : int64
      Line1 : String100
      Line2 : String100 }

and User =
    { Id : int64
      Name : String50
      Email : EmailAddress
      Cpf : Cpf }
