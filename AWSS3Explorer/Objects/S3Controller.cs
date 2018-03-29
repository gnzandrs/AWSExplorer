using System;
using System.Collections.Generic;
using Amazon.S3;

namespace AWSS3Explorer.Objects
{
    public class S3Controller
    {
        int option = 0;
        AmazonS3Client client;
        string bucketName = String.Empty;
        List<string> objectsNames = new List<string>();

        public S3Controller(int option, AmazonS3Client client)
        {
            this.option = option;
            this.client = client;
        }

        public void Run()
        {
            if (option == 1)
            {
                var bucketsNames = ListObjects.ListingBuckets(client);
                Console.WriteLine("Buckets List:");

                foreach (string name in bucketsNames)
                {
                    Console.WriteLine(name);
                }

                Console.Write("Which one you want to see?: ");
                bucketName = Console.ReadLine();

                objectsNames = ListObjects.ListingBucketObjects(client, bucketName);

                foreach (string name in objectsNames)
                {
                    Console.WriteLine(name);
                }
            }
            else if (option == 2)
            {
                // upload
                Console.Write("Destiny bucket name: ");
                bucketName = Console.ReadLine();
                Console.Write("File to upload: ");
                string keyName = Console.ReadLine();
                Console.Write("Path of the file: ");
                string filePath = Console.ReadLine();
                UploadObject uploadObject =
                    new UploadObject(client, bucketName, keyName, filePath);
                bool uploadStatus = uploadObject.Upload();

                if (uploadStatus)
                    Console.WriteLine("Upload OK! ");
                else
                    Console.WriteLine("Error!");
            }
            else if (option == 3)
            {
                // delete
                Console.Write("Which S3 bucket name you want to clean?: ");
                bucketName = Console.ReadLine();
                objectsNames = ListObjects.ListingBucketObjects(client, bucketName);
                DeleteObject deleteObject =
                    new DeleteObject(client, bucketName);
                bool deleteStatus = deleteObject.DeleteAll(objectsNames);

                if (deleteStatus)
                    Console.WriteLine("Clean OK!");
                else
                    Console.WriteLine("Error!");
            }
        }
    }
}
