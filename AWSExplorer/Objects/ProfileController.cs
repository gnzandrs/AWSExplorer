using System;
using System.Collections.Generic;
using Amazon.S3;

namespace AWSS3Explorer.Objects
{
    public class ProfileController
    {
        Profile profile;
        AmazonS3Client client;
        string bucketName = String.Empty;
        List<string> objectsNames = new List<string>();

        public ProfileController(Profile profile, AmazonS3Client client)
        {
            this.profile = profile;
            this.client = client;
        }

        public void Run()
        {
            Console.Write("Insert Profile name: ");
            string profileName = Console.ReadLine();
            Console.Write("Type the access Key: ");
            string accessKey = Console.ReadLine();
            Console.Write("Type the secret key: ");
            string secretKey = Console.ReadLine();

            profile = new Profile(profileName, accessKey, secretKey);
            profile.Create();

            Console.WriteLine("Profile successfully created.");
        }
    }
}
