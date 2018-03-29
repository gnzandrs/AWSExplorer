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
                        var s3Client = AmazonClient.S3Conexion(profile.GetName());
                        var dynamoClient = AmazonClient.DynamoConexion(profile.GetName());

                            switch (userOption)
                            {
                                case -1:
                                    return;
                                case 0:
                                    Environment.Exit(0);
                                    break;
                                case 1:
                                    using (s3Client)
                                    {
                                        var profileController = new ProfileController(profile, s3Client);
                                        profileController.Run();
                                    }
                                    break;
                                case 2:
                                    userOption = Menu.DisplayS3Menu();

                                    using (s3Client)
                                    {
                                        var userController = new S3Controller(userOption, s3Client);
                                        userController.Run();
                                    }

                                    break;
                                case 3:
                                    userOption = Menu.DisplayDynamoMenu();
                                    var dynamoController = new DynamoController(userOption, dynamoClient);
                                    dynamoController.Run();
                                    break;
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
