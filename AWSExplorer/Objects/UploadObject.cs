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
        private string fileLocalPath;
        private string fileRemotePath;

        public UploadObject(AmazonS3Client client, string bucketName, string keyName, 
            string fileLocalPath, string fileRemotePath)
        {
            this.client = client;
            this.bucketName = bucketName;
            this.keyName = keyName;
            this.fileLocalPath = fileLocalPath;
            this.fileRemotePath = fileRemotePath;
        }

        public bool Upload()
        {
            try
            {
                string fullRemotePath = this.fileRemotePath != String.Empty ? 
                    String.Concat(this.fileRemotePath, "/", this.keyName) : 
                    this.keyName;

                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = this.bucketName,
                    Key = fullRemotePath,
                    FilePath = this.fileLocalPath + "\\" + this.keyName,
                    ContentType = "text/plain"
                };

                putRequest.Metadata.Add("x-amz-meta-title", "someTitle");
                PutObjectResponse response = client.PutObject(putRequest);
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
