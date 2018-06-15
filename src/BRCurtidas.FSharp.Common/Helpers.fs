[<AutoOpen>]
module BRCurtidas.FSharp.Helpers

open Chessie.ErrorHandling

let tee f x = f x; x

let stringify x = x.ToString()

let stringifyFailure x = x |> mapFailure (List.map stringify)