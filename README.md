# Amazon Web Services Explorer

AWSExplorer is a terminal toolkit to manage and work with some of the services of aws provide in the cloud. 
This software it is build on C#, so primary was thinked to work on windows to upload data.

You're welcome to add new features and services to work with.

# You need

Windows with .net >= 4.0
An AWS account with a AIM user and a profile file


# Create a AWS profile file

Basically you need a file named "credentials" in your "C:\Users\YourUser\.aws" folder with the AIM credentials.

Example of file:

[testingaws]
aws_access_key_id=exmapleofkeyid
aws_secret_access_key=exampleofsecretaccesskey
region=us-east-1


for more information please read the aws sdk documentation: 
https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-signup.html

