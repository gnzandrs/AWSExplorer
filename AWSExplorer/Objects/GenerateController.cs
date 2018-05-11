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
        static string JsonDirectoryPath => ConfigurationManager.AppSettings["JsonDirectoryPath"];
        int option = 0;
        string sourceTable = String.Empty;
        int recordsAmount = 0;
        int recordsToExtract = 0;

        public GenerateController(int option)
        {
            this.option = option;
        }

        public int CheckForInt(string value)
        {
            bool check = false;
            int valueConvert = 0;

            try
            {   
                while (!check)
                {
                    check = int.TryParse(value, out valueConvert);

                    if (!check)
                    {
                        Console.Write("Not a valid number, please try again: ");
                        value = Console.ReadLine();
                    }
                }

                return valueConvert;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string CheckForString(string value)
        {
            bool check = false;

            try
            {
                while (!check)
                {
                    value = value.Trim();
                    check = (value == String.Empty ? false : true);

                    if (!check)
                    {
                        Console.Write("Not a valid value, please try again: ");
                        value = Console.ReadLine();
                    }
                }

                return value;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        public bool CheckIfTableExist(string tableName)
        {
            try
            {
                bool exists = true;
                SqlConnection conn = new SqlConnection(connectionConfig);
                string query = "SELECT count(*) as Exist from INFORMATION_SCHEMA.TABLES where table_name = '" + tableName + "'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];

                if (dt.Rows[0]["Exist"].ToString() != "1")
                {
                    Console.WriteLine("Table does not exist!");
                    exists = false;
                }

                return exists;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Run()
        {
            if (option == 1)
            {
                try
                {
                    Console.Write("Write the source table name: ");
                    sourceTable = CheckForString(Console.ReadLine());
                    bool tableExists = CheckIfTableExist(sourceTable);

                    if (!tableExists)
                        return;

                    Console.Write("Amount of records to extract (0 = all): ");
                    recordsToExtract = CheckForInt(Console.ReadLine());

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
                    Console.Write("Write a file name: ");
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
                        string path = JsonDirectoryPath + fileName;

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
                            int totalRows = dtFile.Rows.Count;

                            string[] columnNames = dtFile.Columns.Cast<DataColumn>()
                             .Select(x => x.ColumnName)
                             .ToArray();
                            string idColumnName = findIdColumnName(dt, columnNames);
                            string jsonBodyStructure = generateJsonBodyStructure(dt, columnNames);


                            foreach (DataRow dr in dtFile.Rows)
                            {
                                json = "{'type':'add','id':'" + idColumnName + "','fields':";

                                string jsonBodyEdit = jsonBodyStructure;

                                // replace data in structure
                                foreach (string columnName in columnNames)
                                {
                                    string rowData = dr[columnName].ToString();
                                    jsonBodyEdit = jsonBodyEdit.Replace("<" + columnName.ToLower() + ">", rowData);
                                }

                                json += json + jsonBodyEdit;
                                json += "}";

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

        public string findIdColumnName(DataTable dt, string[] columnNames)
        {
            string idColumnName = String.Empty;

            foreach (string columnName in columnNames)
            {
                if (columnName.ToLower().IndexOf("id") != -1)
                {
                    idColumnName = columnName;
                    break;
                }
            }

            return idColumnName;
        }

        public string generateJsonBodyStructure(DataTable data, string[] columnNames)
        {
            int totalRows = data.Rows.Count;
            string jsonBodyStructure = "{";
            int rowCount = 0;

            foreach (string columnName in columnNames)
            {
                jsonBodyStructure += "'" + columnName.ToLower() + "':'<" + columnName.ToLower() + ">'";
                rowCount++;
                if (rowCount < totalRows)
                    jsonBodyStructure += ",";
            }
            jsonBodyStructure += "}";

            return jsonBodyStructure;
        }

        public DataTable ExtractData()
        {
            string amount = (recordsToExtract == 0 ? "*" : recordsToExtract.ToString());
            SqlConnection conn = new SqlConnection(connectionConfig);
            string query = (recordsToExtract > 0 ? "SELECT top " + amount + " * FROM " + this.sourceTable : "SELECT * FROM " + this.sourceTable);
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            DataTable dt = ds.Tables[0];
            return dt;
        }

    }
}
