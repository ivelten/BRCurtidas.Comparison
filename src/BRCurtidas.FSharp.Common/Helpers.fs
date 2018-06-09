namespace BRCurtidas.FSharp

[<AutoOpen>]
module Helpers =
    let tee f x = f x; x