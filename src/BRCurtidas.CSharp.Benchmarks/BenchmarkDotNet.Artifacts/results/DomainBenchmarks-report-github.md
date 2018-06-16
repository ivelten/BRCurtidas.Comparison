``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.201
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-JEVGHD : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT

LaunchCount=1  RunStrategy=ColdStart  TargetCount=1  

```
|                    Method |     Mean | Error |      Gen 0 |     Gen 1 | Allocated |
|-------------------------- |---------:|------:|-----------:|----------:|----------:|
|  GenerateOneHundredOrders |  10.54 s |    NA |  2000.0000 |         - |  11.85 MB |
| GenerateOneThousandOrders | 105.86 s |    NA | 29000.0000 | 2000.0000 | 118.51 MB |
|    ChangeOneHundredOrders |  10.65 s |    NA |  2000.0000 |         - |  11.78 MB |
|   ChangeOneThousandOrders | 106.86 s |    NA | 29000.0000 | 7000.0000 | 117.81 MB |
