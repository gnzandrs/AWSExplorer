using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CargaToctocBundles.Objetos
{
    public static class Menu
    {
        public static int DisplayMenu()
        {
            Console.Clear();
            Console.Title = ".: AWSS3Explorer :.";
            Console.WriteLine(".: AWSS3Explorer :.");
            Console.WriteLine();
            Console.WriteLine("Select option:");
            Console.WriteLine("1. Create a profile file.");
            Console.WriteLine("2. List objects.");
            Console.WriteLine("3. Upload.");
            Console.WriteLine("4. Delete.");
            Console.WriteLine("0. Exit");

            try
            {
                string response = Console.ReadLine();
                return int.Parse(response);
            }
            catch (Exception)
            {
                Console.WriteLine("No valid option!");
                return -1;
            }
        }


    }
}
