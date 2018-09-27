using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CargaToctocBundles.Objetos
{
    public static class Menu
    {
        public static int DisplayMainMenu()
        {
            Console.Clear();
            Console.Title = ".: AWSExplorer :.";
            Console.WriteLine(".: AWSExplorer :.");
            Console.WriteLine();
            Console.WriteLine("Select option:");
            Console.WriteLine("1. Create a profile file.");
            Console.WriteLine("2. S3.");
            Console.WriteLine("3. DynamoDB.");
            Console.WriteLine("4. Data Generation.");
            Console.WriteLine("5. Log Configuration.");
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

        public static int DisplayS3Menu()
        {
            Console.Clear();
            Console.Title = ".: AWSExplorer :.";
            Console.WriteLine(".: AWSExplorer :.");
            Console.WriteLine();
            Console.WriteLine("Select S3 option:");
            Console.WriteLine("1. List objects.");
            Console.WriteLine("2. Upload objects.");
            Console.WriteLine("3. Delete objects.");
            Console.WriteLine("0. Return");

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

        public static int DisplayDynamoMenu()
        {
            Console.Clear();
            Console.Title = ".: AWSExplorer :.";
            Console.WriteLine(".: AWSExplorer :.");
            Console.WriteLine();
            Console.WriteLine("Select DynamoDB option:");
            Console.WriteLine("1 Export SqlServer to DynamoDB");
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

        public static int DisplayGenerateMenu()
        {
            Console.Clear();
            Console.Title = ".: AWSExplorer :.";
            Console.WriteLine(".: AWSExplorer :.");
            Console.WriteLine();
            Console.WriteLine("Select the option:");
            Console.WriteLine("1 Export Table Data as JSON");
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

        public static int DisplayLogMenu()
        {
            Console.Clear();
            Console.Title = ".: AWSExplorer :.";
            Console.WriteLine(".: AWSExplorer :.");
            Console.WriteLine();
            Console.WriteLine("Select the option:");
            Console.WriteLine("1. Show");
            Console.WriteLine("2. Change");

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
