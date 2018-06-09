module BRCurtidas.FSharp.Tests.Helpers

open Xunit

let pass = ()

let fail msg = failwith msg

let equals actual expected = Assert.Equal(expected, actual)