using NHibernate.Linq;
using NHibernateStart.Domain;
using NHibernateStart.Services;
using System;
using System.Linq;

namespace NHibernateStart
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: First run ScriptToCreateDatabase.sql (in Script-folder) in your database

            var session = DbService.OpenSession();

            // Remove all customers

            foreach (var c in session.Query<Customer>())
            {
                session.Delete(c);
            }

            // Save one customer

            var customer1 = new Customer
            {
                FirstName = "Rick"
            };

            var customer2 = new Customer
            {
                FirstName = "Liiisa"
            };

            var customer3 = new Customer
            {
                FirstName = "Morty"
            };

            session.Save(customer1);
            session.Save(customer2);
            session.Save(customer3);

            // Update Liiisa => Lisa

            var person = session.Query<Customer>().Where(c => c.FirstName == "Liiisa").Single();
            person.FirstName = "Lisa";
            session.Save(person);

            // Report stuff from the database

            var allCustomer = session.Query<Customer>().ToList();

            Console.WriteLine($"Nr of customers: {allCustomer.Count()}");
            Console.WriteLine($"Name of second customer in list: {allCustomer[1].FirstName}");

            var result = session.Query<Customer>().Where(c => c.FirstName.StartsWith("M")).First();
            Console.WriteLine($"One customer that start with M: {result.FirstName}");


            DbService.CloseSession(session);

            Console.ReadKey();
        }
    }
}
