﻿using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.DynamoDBv2;

namespace AWSS3Explorer.Objects
{
    public static class AmazonClient
    {
        static CredentialProfile basicProfile;
        static AWSCredentials awsCredentials;

        public static AmazonS3Client S3Conexion(string profileName)
        {
            var sharedFile = new SharedCredentialsFile();

            if (sharedFile.TryGetProfile(profileName, out basicProfile) &&
                AWSCredentialsFactory.TryGetAWSCredentials(basicProfile, sharedFile, out awsCredentials))
            {
                return new AmazonS3Client(awsCredentials, basicProfile.Region);
            }
            else {
                return null;
            }
        }

        public static AmazonDynamoDBClient DynamoConexion(string profileName)
        {
            var sharedFile = new SharedCredentialsFile();

            if (sharedFile.TryGetProfile(profileName, out basicProfile) &&
                AWSCredentialsFactory.TryGetAWSCredentials(basicProfile, sharedFile, out awsCredentials))
            {
                return new AmazonDynamoDBClient(awsCredentials, basicProfile.Region);
            }
            else
            {
                return null;
            }
        }
    }
}
