using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant_sGuild {
    class Program {
        static void Main(string[] args) {
            String select;
            Int16 numMerch, amDeals;

            Console.WriteLine("\t### Welcome to Merchant's Guild!!! ###");

            Console.Write("Enter number (is a multiply of 6) of merchants in guild: ");
            numMerch = Convert.ToInt16(Console.ReadLine());
            while (numMerch % 6 != 0 && numMerch <= 0) {
                Console.WriteLine("Wrong number!");

                Console.Write("Enter count (is a multiply of 6) of merchants in guild: ");
                numMerch = Convert.ToInt16(Console.ReadLine());
            }

            Console.Write("Enter amount (from 5 to 10) of deals between each pair of merchants: ");
            amDeals = Convert.ToInt16(Console.ReadLine());
            while (amDeals < 5 || amDeals > 10) {
                Console.WriteLine("Wrong number!");

                Console.Write("Enter amount (from 5 to 10) of deals between each pair of merchants: ");
                amDeals = Convert.ToInt16(Console.ReadLine());
            }

            Guild guild = new Guild(numMerch, amDeals);

            Console.WriteLine();
            Console.WriteLine("Guild:");
            guild.PrintAllMerchants();

            Console.WriteLine("Press any key to start first year...");
            Console.ReadLine();

            do {
                guild.StartNewYear();
                guild.SummingUpYear();

                Console.WriteLine("Start the " + guild.Age + " year? (y/n)");
                select = Console.ReadLine();
                while (select != "n" && select != "N" && select != "y" && select != "Y") {
                    Console.WriteLine("Wrong symbol!");
                    Console.WriteLine("Start the " + guild.Age + " year? (y/n)");
                    select = Console.ReadLine();
                }
            } while (select != "N" && select != "n");
        }
    }
}
