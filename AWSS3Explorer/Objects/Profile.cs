using Amazon;
using Amazon.Runtime.CredentialManagement;
using System;
using System.IO;

namespace AWSS3Explorer.Objects
{
    public class Profile
    {
        private string profileName;
        private string accessKey;
        private string secretKey;
        private string windowsUserPath;

        public Profile()
        {

        }

        public Profile(string profileName, string accessKey, string secretKey)
        {
            this.profileName = profileName;
            this.accessKey = accessKey;
            this.secretKey = secretKey;
        }

        public string GetName()
        {
            return this.profileName;
        }

        public void Create()
        {
            var options = new CredentialProfileOptions
            {
                AccessKey = this.accessKey,
                SecretKey = this.secretKey
            };
  
            var profile = new CredentialProfile(this.profileName, options);
            profile.Region = RegionEndpoint.USEast1;
            var sharedFile = new SharedCredentialsFile();
            sharedFile.RegisterProfile(profile);
        }
        
        public bool DirectorySearch()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            path += "\\.aws\\credentials";

            if (File.Exists(path))
            {
                this.windowsUserPath = path;
                return true;
            }
            else {
                return false;
            }
        }

        public void FileRead()
        {
            using (StreamReader sr = File.OpenText(this.windowsUserPath))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Contains("["))
                    {
                        int large = s.Length;
                        int init = s.IndexOf("[") + 1;
                        int final = Math.Abs(large - init);
                        this.profileName = s.Substring(init, (final-1));
                    }

                    if (s.Contains("aws_access_key_id"))
                    {
                        int large = s.Length;
                        int init = s.IndexOf("=") + 1;
                        int final = Math.Abs(large - init);
                        this.accessKey = s.Substring(init, final);
                    }

                    if (s.Contains("aws_secret_access_key"))
                    {
                        int large = s.Length;
                        int init = s.IndexOf("=") + 1;
                        int final = Math.Abs(large - init);
                        this.secretKey = s.Substring(init, final);
                    }
                }
            }
        }

    }
}
