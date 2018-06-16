namespace BRCurtidas.FSharp.Benchmarks

open BRCurtidas.FSharp.Data
open BenchmarkDotNet.Attributes
open Chessie.ErrorHandling
open BenchmarkDotNet.Attributes.Jobs
open BenchmarkDotNet.Engines

[<MemoryDiagnoser>]
[<SimpleJob(RunStrategy.ColdStart, launchCount = 1, targetCount = 1)>]
type DomainBenchmarks() =
    let mutable orders : Entity<Order> list = []

    let errorMsg errors = sprintf "Error. Generated entity must not result in a failure, but resulted in: %A" errors

    let generateOrders =
        let rec loop current orders max =
            if current > max then
                orders
            else
                let entity = trial {
                    let! account = Account.create "ivelten" "password"
                    let! address = Address.create "Rua M. Francisco de Moura, 251" (Some "CS4")
                    let! user = User.create "Ismael" "ismaelcarlosvelten@gmail.com" "105.643.607-75" "21980836100" account address
                    let! product = Product.create 21.5M true "Pacote com curtidas nacionais no Facebook" "Curtidas Facebook Nacionais"
                    let payment = Some { Reference = "12345F"; Method = PaymentMethod.PayPal }
                    let! network = SocialNetworkAccount.create SocialNetwork.Facebook "https://www.facebook.com/ismaelcarlosvelten"
                    let! order = Order.create user product payment OrderStatus.WaitingPayment 200 network
                    return! Entity.create current order }
                match entity with
                | Ok (entity, _) -> loop (current + 1L) (entity :: orders) max
                | Bad errors -> failwith (errorMsg errors)
        loop 1L []

    let setAccount (entity : Entity<Order>) = trial {
        let! account = Account.create "aanjos" "password2"
        let! user = User.create "André" "aanjos@gmail.com" "158.844.757-05" "21965448522" account entity.Value.User.Address
        return { entity with Value = { entity.Value with User = user } } }

    let changeOrders orders =
        orders
        |> List.map (fun x -> setAccount x)
        |> List.map (fun x ->
            match x with
            | Ok (entity, _) -> entity
            | Bad errors -> failwith (errorMsg errors))

    [<Benchmark>]
    member __.GenerateOneHundredOrders() =
        orders <- generateOrders 100L

    [<Benchmark>]
    member __.GenerateOneThousandOrders() =
        orders <- generateOrders 1000L

    [<GlobalSetup(Target = "ChangeOneHundredOrders")>]
    member __.SetupChangeOneHundredOrders() =
        orders <- generateOrders 100L

    [<Benchmark>]
    member __.ChangeOneHundredOrders() =
        orders <- changeOrders orders

    [<GlobalSetup(Target = "ChangeOneThousandOrders")>]
    member __.SetupChangeChangeOneThousandOrders() =
        orders <- generateOrders 1000L

    [<Benchmark>]
    member __.ChangeOneThousandOrders() =
        orders <- changeOrders orders