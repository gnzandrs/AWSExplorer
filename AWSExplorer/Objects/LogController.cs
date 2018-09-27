using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Configuration;

namespace AWSS3Explorer.Objects
{
    class LogController
    {
        static string appConfigKey = "DynamoLogTable";
        int option = 0;

        public LogController(int option)
        {
            this.option = option;
        }

        public void Run()
        {
            // show
            if (option == 1)
            {
                string[] dynamoConfig = ReadConfig();
                if (dynamoConfig != null)
                {
                    Console.WriteLine(".: Active Configuration :.");
                    Console.WriteLine("activated: " + dynamoConfig[0]);
                    Console.WriteLine("table for log: " + dynamoConfig[1]);
                }
            }
            // config
            else if (option == 2)
            {
                string[] dynamoConfig = ReadConfig();
                string read = String.Empty;

                if (dynamoConfig != null)
                {
                    string activated = String.Empty;
                    string tableName = String.Empty;
                        
                    // activate
                    Console.Write("You wish to activate log? (Y/N): ");
                    read = Console.ReadLine();

                    if (CheckForValidOption(read))
                    {
                        activated = read.ToLower() == "s" ? "true" : "false";
                    }
                    else {
                        Console.WriteLine("Invalid option, try again!");
                        return;
                    }

                    // table name
                    Console.WriteLine("Write the destination table name: ");
                    read = Console.ReadLine();
                    tableName = read;

                    // update config file
                    string newConfig = String.Concat(activated, ';', tableName);
                    this.UpdateConfig(newConfig);
                    Console.WriteLine("Configuration Ok!");
                }
            }
            else {
                Console.WriteLine("Invalid Option.");
                return;
            }
        }

        public bool CheckForValidOption(string chosenOption)
        {
            try
            {
                char[] validOptions = { 'Y', 'N' };
                bool parse = false;
                char option = 'N';

                chosenOption = chosenOption.ToUpper();
                parse = Char.TryParse(chosenOption, out option);

                if (parse)
                {
                    foreach (char validOption in validOptions)
                    {
                        if (option == validOption)
                            return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string[] ReadConfig()
        {
            string[] configKeys = null;
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string dynamoConfig = config.AppSettings.Settings[appConfigKey].Value;
                configKeys = dynamoConfig.Split(';');
            }
            catch (Exception)
            {
                Console.WriteLine("There is a problem reading the configuration.");
            }

            return configKeys;
        }

        public bool UpdateConfig(string newConfigString)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove(appConfigKey);
                config.AppSettings.Settings.Add(appConfigKey, newConfigString);
                config.Save(ConfigurationSaveMode.Minimal);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
