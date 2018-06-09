module BRCurtidas.FSharp.Tests.Helpers

open Xunit

let pass = ()

let fail msg = failwith msg

let equals (actual : obj) (expected : obj) =
    Assert.Equal(expected, actual)

let contains (items : 'a seq) (expected : 'a) =
    Assert.Contains(expected, items)

let isEmpty (items : 'a seq) =
    Assert.Empty(items)