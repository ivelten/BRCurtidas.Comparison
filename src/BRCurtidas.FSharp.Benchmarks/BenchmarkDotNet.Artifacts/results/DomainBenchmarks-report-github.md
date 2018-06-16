``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.201
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT DEBUG
  Job-BJJLBY : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT

LaunchCount=1  RunStrategy=ColdStart  TargetCount=1  

```
|                    Method |     Mean | Error |      Gen 0 |     Gen 1 | Allocated |
|-------------------------- |---------:|------:|-----------:|----------:|----------:|
|  GenerateOneHundredOrders |  10.74 s |    NA |  3000.0000 |         - |  12.44 MB |
| GenerateOneThousandOrders | 107.04 s |    NA | 31000.0000 | 2000.0000 | 124.36 MB |
|    ChangeOneHundredOrders |  10.69 s |    NA |  3000.0000 |         - |  12.05 MB |
|   ChangeOneThousandOrders | 107.60 s |    NA | 30000.0000 | 2000.0000 | 120.53 MB |
