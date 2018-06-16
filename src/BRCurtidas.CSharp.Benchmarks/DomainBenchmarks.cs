using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using BRCurtidas.CSharp.Common.DomainTypes;
using BRCurtidas.CSharp.Data;
using System;
using System.Collections.Generic;

namespace BRCurtidas.CSharp.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, targetCount: 1)]
    public class DomainBenchmarks
    {
        private readonly List<Entity<Order>> orders = new List<Entity<Order>>();

        private void GenerateOrders(List<Entity<Order>> orders, long max)
        {
            for (long i = 1; i <= max; i++)
            {
                var product = new Product
                {
                    Price = new Currency(21.5M),
                    Description = new String100("Pacote com curtidas nacionais no Facebook"),
                    Title = new String50("Curtidas Facebook Nacionais"),
                    Enabled = true
                };

                var account = new Account
                {
                    UserName = new UserName("ivelten"),
                    PasswordHash = PasswordHash.FromPassword("password")
                };

                var address = new Address
                {
                    Line1 = new String100("Rua M. Francisco de Moura, 251"),
                    Line2 = new String100("CS4")
                };

                var user = new User
                {
                    Account = account,
                    Address = address,
                    Cpf = new Cpf("105.643.607-75"),
                    Email = new EmailAddress("ismaelcarlosvelten@gmail.com"),
                    Name = new String50("Ismael"),
                    Phone = new PhoneNumber("21980836100")
                };

                var payment = new Payment
                {
                    Reference = "12345F",
                    Method = PaymentMethod.PayPal
                };

                var socialNetworkAccount = new SocialNetworkAccount
                {
                    Profile = new Url("https://www.facebook.com/ismaelcarlosvelten"),
                    Type = SocialNetworkType.Facebook
                };

                var qty = new Quantity(200);
                var price = new Currency(qty.Value * product.Price.Value);

                var order = new Order
                {
                    User = user,
                    Payment = payment,
                    Product = product,
                    Quantity = qty,
                    Price = price,
                    SocialNetworkAccount = socialNetworkAccount,
                    Status = OrderStatus.WaitingPayment,
                    Created = DateTime.Now
                };

                var entity = new Entity<Order>
                {
                    Id = new Id(i),
                    Value = order
                };

                orders.Add(entity);
            }
        }

        private void ChangeOrders(List<Entity<Order>> orders)
        {
            foreach (var order in orders)
            {
                order.Value.User.Name = new String50("André");
                order.Value.User.Email = new EmailAddress("aanjos@gmail.com");
                order.Value.User.Cpf = new Cpf("158.844.757-05");
                order.Value.User.Phone = new PhoneNumber("21965448522");
                order.Value.User.Account.UserName = new UserName("aanjos");
                order.Value.User.Account.PasswordHash = PasswordHash.FromPassword("password2");
            }
        }

        [Benchmark]
        public void GenerateOneHundredOrders()
        {
            GenerateOrders(orders, 100L);
        }

        [Benchmark]
        public void GenerateOneThousandOrders()
        {
            GenerateOrders(orders, 1000L);
        }

        [GlobalSetup(Target = nameof(ChangeOneHundredOrders))]
        public void SetupChangeOneHundredOrders()
        {
            GenerateOrders(orders, 100L);
        }

        [Benchmark]
        public void ChangeOneHundredOrders()
        {
            ChangeOrders(orders);
        }

        [GlobalSetup(Target = nameof(ChangeOneThousandOrders))]
        public void SetupChangeOneThousandOrders()
        {
            GenerateOrders(orders, 1000L);
        }

        [Benchmark]
        public void ChangeOneThousandOrders()
        {
            ChangeOrders(orders);
        }
    }
}