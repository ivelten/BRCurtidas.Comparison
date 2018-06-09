namespace BRCurtidas.FSharp.Data

type Account =
    { Id : int64
      UserName : string
      PasswordHash : string }

and Address =
    { Id : int64
      Line1 : string
      Line2 : string }
