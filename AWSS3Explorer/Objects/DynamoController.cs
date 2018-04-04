using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace AWSS3Explorer.Objects
{
    class DynamoController
    {
        static AmazonDynamoDBClient client;
        static string connectionConfig => ConfigurationManager.ConnectionStrings["SqlServerConnection"].ConnectionString;
        int option = 0;
        string sourceTable = String.Empty;
        string sourcePk = String.Empty;
        string destinyTable = String.Empty;
        string destinyPk = String.Empty;

        public DynamoController(int option, AmazonDynamoDBClient dynamoClient)
        {
            this.option = option;
            client = dynamoClient;
        }

        public void Run()
        {
            if (option == 1)
            {
                try
                {
                    AmazonDynamoDBConfig config = new AmazonDynamoDBConfig();
                    config.ServiceURL = "http://dynamodb.us-west-2.amazonaws.com";
                    TransferData();
                }
                catch (AmazonDynamoDBException e) { Console.WriteLine("DynamoDB Message:" + e.Message); }
                catch (AmazonServiceException e) { Console.WriteLine("Service Exception:" + e.Message); }
                catch (Exception e) { Console.WriteLine("General Exception:" + e.Message); }
                Console.ReadLine();
            }
        }

        private void TransferData()
        {
            Console.Write("Source Table: ");
            sourceTable = Console.ReadLine();
            Console.WriteLine("PrimaryKey from source Table: ");
            sourcePk = Console.ReadLine();

            Console.Write("Destiny Table: ");
            destinyTable = Console.ReadLine();
            Console.WriteLine("PrimaryKey from Destiny Table: ");
            destinyPk = Console.ReadLine();

            if (String.IsNullOrEmpty(sourceTable)
                && String.IsNullOrEmpty(sourcePk)
                && String.IsNullOrEmpty(destinyTable)
                && String.IsNullOrEmpty(destinyPk))
            {
                Console.WriteLine("Reading Source Data");
                SqlConnection conn = new SqlConnection(connectionConfig);
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM " + sourceTable, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                Console.WriteLine("Opening destination table at AWS");


                Table destTable = Table.LoadTable(client, destinyTable);
                int r = 0; 

                foreach (DataRow dr in dt.Rows)
                {
                    Console.WriteLine("Starting Insert Row: " + r.ToString());
                    var doc = new Document();
                    doc[destinyPk] = Convert.ToString(dr[sourcePk]);
                    int c = 0;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        if ((dt.Rows[r][c] != null) && (dc.ColumnName != sourcePk))
                        {
                            Console.WriteLine(dc.ColumnName + "  " + Convert.ToString(dt.Rows[r][c]));
                            doc[dc.ColumnName] = Convert.ToString(dt.Rows[r][c]);
                        }
                        c++;
                    }
                    Console.WriteLine("Completed Insert Row: " + r.ToString());
                    destTable.PutItem(doc);
                    r++;
                }
                Console.WriteLine("Data uploaded... To continue, press enter");
            }
            else {
                Console.WriteLine("You have to enter all the required data.");
            }
        }
        private static void UploadData()
        {
            Table sampleTable = Table.LoadTable(client, "SampleData");
            var d1 = new Document();
            d1["id"] = "1";
            d1["Field1"] = "A field";
            d1["Field2"] = "Another Field";
            sampleTable.PutItem(d1);
            var d2 = new Document();
            d2["id"] = "2";
            d2["Field1"] = "A field 2";
            d2["Field2"] = "Another Field 2";
            sampleTable.PutItem(d2);
        }

    }
}
