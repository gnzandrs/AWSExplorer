using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace AWSS3Explorer.Objects
{
    class GenerateController
    {
        static string connectionConfig => ConfigurationManager.ConnectionStrings["SqlServerConnection"].ConnectionString;
        int option = 0;
        string sourceTable = "TableName";
        int recordsAmount = 0;

        public GenerateController(int option)
        {
            this.option = option;
        }

        public void Run()
        {
            if (option == 1)
            {
                try
                {
                    Console.WriteLine("Extracting data from SQL Server");
                    DataTable dt = ExtractData();

                    int totalData = dt.Rows.Count;
                    int limit = 30000; // 5 MB limited by amazon
                    double totalOfFiles = ((double)totalData / (double)limit);
                    int filesToCreate = Convert.ToInt32(Math.Ceiling(totalOfFiles));
                    DataTable dtFile = null;
                    int currentRecord = 0;
                    string currentDate = DateTime.Now.ToString("yyyyMMdd");
                    string prefixFileName = String.Empty;
                    Console.WriteLine("Total of files to create: " + filesToCreate);
                    Console.WriteLine("Write a File Name: ");
                    prefixFileName = Console.ReadLine();

                    for (int i = 1; i <= filesToCreate; i++)
                    {
                        dtFile = dt.Clone();

                        // charge dt rows
                        for (int rowNumber = 0; rowNumber < dt.Rows.Count; rowNumber++)
                        {
                            if ((rowNumber < limit) && (currentRecord < totalData))
                            {
                                dtFile.ImportRow(dt.Rows[currentRecord]);
                                currentRecord++;
                            }
                            else {
                                break;
                            }
                        }

                        Console.WriteLine("Creating file " + i);

                        // create and write file
                        string fileName = prefixFileName + "_" + i + "_" + currentDate + ".json";
                        string path = @"E:\data\" + fileName;

                        if (!File.Exists(path))
                        {
                            var file = File.Create(path);
                            file.Close();
                            file.Dispose();
                        }

                        using (var tw = new StreamWriter(path, true))
                        {
                            string json = String.Empty;
                            json = "[";
                            tw.Write(json);

                            int lastLine = dtFile.Rows.Count;
                            int actualLine = 1;

                            foreach (DataRow dr in dtFile.Rows)
                            {
                                json = "{\"type\":\"add\",\"id\":\"" + dr["IdOfTable"] + "\",\"fields\":{\"IdOfTable\":\"" 
                                    + dr["IdOfTable"] + "\",\"ExampleField\":\"" + dr["ExampleField"] + "\"}}";

                                if (actualLine < dtFile.Rows.Count)
                                {
                                    json += ",";
                                }

                                tw.WriteLine(json);
                                actualLine++;
                            }
                            json = "];";
                            tw.WriteLine(json);
                        }

                        Console.WriteLine("File " + fileName + "... OK");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public string CreateFile()
        {
            return String.Empty;
        }

        public DataTable ExtractData()
        {
            Console.WriteLine("Reading Source Data");
            SqlConnection conn = new SqlConnection(connectionConfig);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM " + this.sourceTable, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            DataTable dt = ds.Tables[0];
            return dt;
        }

    }
}
