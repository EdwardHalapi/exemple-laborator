using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using static Exemple.Domain.Models.Carucior;
using static Exemple.Domain.PriceOperations;
using Exemple.Domain;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Net.Http;
using Exemple.Data.Repositories;
using Exemple.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MainProgram
{
    class Program
    {
        private static readonly Random random = new Random();
        private static string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=C:\\PSSC LAB\\EXEMPLE-LABORATOR\\LUCRAREA-01\\EXEMPLE\\MAINPROG\\DATABASE1.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        static async Task Main(string[] args)
        {
            using ILoggerFactory loggerFactory = ConfigureLoggerFactory();
            ILogger<PublishProductWorkflow> logger = loggerFactory.CreateLogger<PublishProductWorkflow>();
            var productCode = ProductCode.TryParse("1A2B3C5556789");
            var productExists = await productCode.Match(
                Some: prodCOd => CheckProductExists(prodCOd).Match(Succ: value => value, exception => false),
                None: () => Task.FromResult(false)
            );

            var resultProdCode = from prodCode in ProductCode.TryParse("1A2B3C5556789")
                                                    .ToEitherAsync(() => "Invlid product code.")
                         from exists in CheckProductExists(prodCode)
                                                    .ToEither(ex =>
                                                    {
                                                        Console.Error.WriteLine(ex.ToString());
                                                        return "Could not validate product code";
                                                    })
                         select exists;

            await resultProdCode.Match(
                 Left: message => Console.WriteLine(message),
                 Right: flag => Console.WriteLine(flag));
            var dbContextBuilder = new DbContextOptionsBuilder<OrderContext>()
                                              .UseSqlServer(ConnectionString)
                                              .UseLoggerFactory(loggerFactory);
            OrderContext orderContext = new OrderContext(dbContextBuilder.Options);
            ProductRepository productsRepository = new(orderContext);
            OrderRepository orderRepository = new(orderContext);
            var listOfProducts = ReadListOfProducts().ToArray();
            PublishQuantityCommand command = new(listOfProducts);
            PublishProductWorkflow workflow = new();
            var result = await workflow.ExecuteAsync(command, CheckProductExists);
           
            result.Match(
                    whenPaidCaruciorFaildEvent: @event =>
                    {
                        Console.WriteLine($"Publish failed: {@event.Reason}");
                        return @event;
                    },
                    whenPaidCaruciorScucceededEvent: @event =>
                    {
                        Console.WriteLine($"Publish succeeded.");
                        return @event;
                    }
                );

            Console.WriteLine("Try again later!");
        }
        private static ILoggerFactory ConfigureLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
                                builder.AddSimpleConsole(options =>
                                {
                                    options.IncludeScopes = true;
                                    options.SingleLine = true;
                                    options.TimestampFormat = "hh:mm:ss ";
                                })
                                .AddProvider(new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()));
        }
        private static TryAsync<bool> CheckProductExists(ProductCode code)
        {
            Func<Task<bool>> func = async () =>
            {
                bool flag = false;
                if(code.Value != null)
                {

                    if (code.Value.StartsWith("1A2B3C"))
                    {
                        flag = true;
                    }

                }
                return flag;
            };
            return TryAsync(func);
        }

        private static List<UnvalidatedProductQuantity> ReadListOfProducts()
        {
            List<UnvalidatedProductQuantity> listOfProducts = new();
            string flag = "N";
            do
            {
                var productCode = ReadValue("Introduce Product Code: ");
                while (string.IsNullOrEmpty(productCode))
                {
                    productCode = ReadValue("Product Code must not be blank: ");
                }

                var quantity = ReadValue("Quantity: ");
                while (string.IsNullOrEmpty(quantity))
                {
                    quantity = ReadValue("Quantity must not be blank: ");
                }

                var address = ReadValue("Address: ");
                while (string.IsNullOrEmpty(address))
                {
                    address = ReadValue("Address must not be blank: ");
                }

                listOfProducts.Add(new(productCode, quantity, address));
                flag = ReadValue("Do you want to add more items [Y/N]");
            } while (flag != "N");
            return listOfProducts;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
