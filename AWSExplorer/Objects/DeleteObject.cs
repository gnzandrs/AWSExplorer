using System.Collections.Generic;
using Amazon.S3;
using Amazon.S3.Model;

namespace AWSS3Explorer.Objects
{
    class DeleteObject
    {
        private AmazonS3Client client;
        private string bucketName;
        public DeleteObject(AmazonS3Client client, string bucketName)
        {
            this.client = client;
            this.bucketName = bucketName;
        }

        public bool DeleteAll(List<string> objects)
        {
            DeleteObjectsRequest multiObjectDeleteRequest = new DeleteObjectsRequest();
            multiObjectDeleteRequest.BucketName = this.bucketName;

            foreach (string objectName in objects)
            {
                multiObjectDeleteRequest.AddKey(objectName, null);
            }

            try
            {
                DeleteObjectsResponse response = this.client.DeleteObjects(multiObjectDeleteRequest);
                //Console.WriteLine("Successfully deleted all the {0} items", response.DeletedObjects.Count);
                return true;
            }
            catch (DeleteObjectsException e)
            {
                // Process exception.
                return false;
            }
        }
    }
}
