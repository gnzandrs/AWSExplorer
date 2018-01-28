using System;
using Amazon.S3;
using Amazon.S3.Model;

namespace AWSS3Explorer.Objects
{
    class UploadObject
    {
        private AmazonS3Client client;
        private string bucketName;
        private string keyName;
        private string filePath;

        public UploadObject(AmazonS3Client client, string bucketName, string keyName, string filePath)
        {
            this.client = client;
            this.bucketName = bucketName;
            this.keyName = keyName;
            this.filePath = filePath;
        }

        public bool Upload()
        {
            try
            {
                PutObjectRequest putRequest1 = new PutObjectRequest
                {
                    BucketName = this.bucketName,
                    Key = this.keyName,
                    ContentBody = "sample text"
                };

                PutObjectResponse response1 = client.PutObject(putRequest1);

                PutObjectRequest putRequest2 = new PutObjectRequest
                {
                    BucketName = this.bucketName,
                    Key = this. keyName,
                    FilePath = this.filePath + "\\" + this.keyName,
                    ContentType = "text/plain"
                };

                putRequest2.Metadata.Add("x-amz-meta-title", "someTitle");

                PutObjectResponse response2 = client.PutObject(putRequest2);

                return true;

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
                        "For service sign up go to http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine(
                        "Error occurred. Message:'{0}' when writing an object"
                        , amazonS3Exception.Message);
                }

                return false;
            }
        }
    }
}
