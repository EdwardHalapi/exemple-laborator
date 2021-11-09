using Exemple.Domain;
using System;
using System.Collections.Generic;
using static Exemple.Domain.Carucior;

namespace Exemple
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var listOfGrades = ReadListOfGrades().ToArray();
            EmptyCarucior unvalidatedGrades = new(listOfGrades);
            ICarucior result = ValidateCarucior(unvalidatedGrades);
            result.Match(
                whenEmptyCarucior: unvalidatedResult => unvalidatedGrades,
                whenPaidCarucior: publishedResult => publishedResult,
                whenInvalidatedCarucior: invalidResult => invalidResult,
                whenValidatedCarucior: validatedResult => PublishCarucior(validatedResult)
            );

            Console.WriteLine("Hello World!");
        }

        private static List<UnvalidatedProduct> ReadListOfGrades()
        {
            List <UnvalidatedProduct> listOfGrades = new();
            do
            {
                var registrationNumber = ReadValue("Product Code: ");
                if (string.IsNullOrEmpty(registrationNumber))
                {
                    break;
                }

                var grade = ReadValue("Quantity: ");
                if (string.IsNullOrEmpty(grade))
                {
                    break;
                }

                listOfGrades.Add(new (registrationNumber, grade));
            } while (true);
            return listOfGrades;
        }

        private static ICarucior ValidateCarucior(EmptyCarucior unvalidatedGrades) =>
            random.Next(100) > 50 ?
            new InvalidatedCarucior(new List<UnvalidatedProduct>(), "Random errror")
            : new ValidatedCarucior(new List<ValidatedProduct>());

        private static ICarucior PublishCarucior(ValidatedCarucior validCarucior) =>
            new PaidCarucior(new List<ValidatedProduct>(), DateTime.Now);

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
