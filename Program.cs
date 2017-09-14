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
            // Skapa databasen utifrån mappningsfil
            DbService.CreateDatabase();
            Console.ReadKey();
        }
    }
}
