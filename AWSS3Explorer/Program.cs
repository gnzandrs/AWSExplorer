using AWSS3Explorer.Objects;
using CargaToctocBundles.Objetos;
using System;
using System.Collections.Generic;

namespace AWSS3Explorer
{
    class Program
    {
        static void Main(string[] args)
        {
            string resp = String.Empty;
            int userOption = 0;
            var profile = new Profile();

            if (profile.DirectorySearch())
            {
                profile.FileRead();
                Console.Write("Profile " + profile.GetName() + " detected on the system. You want to use it? (Y/N)");
                resp = Console.ReadLine();

                if (resp.ToLower() != "y")
                {
                    Console.WriteLine("You must create a profile before use the program.");
                }
                else
                {
                    do
                    {
                        userOption = Menu.DisplayMainMenu();
                        var client = AmazonClient.Conexion(profile.GetName());

                        using (client)
                        {
                            switch (userOption)
                            {
                                case -1:
                                    return;
                                case 0:
                                    Environment.Exit(0);
                                    break;
                                case 1:
                                    var profileController = new ProfileController(profile, client);
                                    profileController.Run();
                                    break;
                                case 2:
                                    userOption = Menu.DisplayS3Menu();
                                    var userController = new S3Controller(userOption, client);
                                    userController.Run();
                                    break;
                            }
                        }

                        Console.WriteLine("Press M to get back to main menu.");
                        resp = Console.ReadLine();

                    }
                    while (resp == "m");
                }
            }
        }
    }
}
