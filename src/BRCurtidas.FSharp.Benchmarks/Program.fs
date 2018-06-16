namespace BRCurtidas.FSharp.Benchmarks

open BenchmarkDotNet.Running

module Program =
    [<EntryPoint>]
    let main _ =
        BenchmarkRunner.Run<DomainBenchmarks>() |> ignore
        0