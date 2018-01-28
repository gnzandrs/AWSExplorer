using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;

namespace AWSS3Explorer.Objects
{
    public static class ListObjects
    {
        public static List<string> ListingBuckets(AmazonS3Client client)
        {
            var response = client.ListBuckets();
            List<Amazon.S3.Model.S3Bucket> buckets = response.Buckets;
            List<string> names = new List<string>();

            foreach (Amazon.S3.Model.S3Bucket bucket in buckets)
            {
                names.Add(bucket.BucketName);
            }

            return names;
        }

        public static List<string> ListingBucketObjects(AmazonS3Client client, string bucketName)
        {
            List<string> names = new List<string>();

            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    MaxKeys = 10
                };

                ListObjectsV2Response response;
                do
                {
                    response = client.ListObjectsV2(request);

                    // Process response.
                    foreach (S3Object entry in response.S3Objects)
                    {
                        names.Add(entry.Key);
                    }

                    request.ContinuationToken = response.NextContinuationToken;

                    return names;

                } while (response.IsTruncated == true);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                    Console.WriteLine(
                    "To sign up for service, go to http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine(
                     "Error occurred. Message:'{0}' when listing objects",
                     amazonS3Exception.Message);
                }

                return null;
            }
        }

    }
}
