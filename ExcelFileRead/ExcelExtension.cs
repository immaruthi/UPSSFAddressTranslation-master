using ExcelDataReader;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExcelFileRead
{

    public class ExcelExtensionReponse
    {
        public string ExcelExtensionReponseData { get; set; }

        public bool Response { get; set; }

        public Exception exception { get; set; }
    }

    public class ExcelExtension
    {


        //public static DataSet Parse(string fileName)
        //{
        //    string connectionString = string.Format("provider=Microsoft.Jet.OLEDB.4.0; data source={0};Extended Properties=Excel 8.0;", fileName);


        //    DataSet data = new DataSet();

        //    foreach (var sheetName in GetExcelSheetNames(connectionString))
        //    {
        //        using (OleDbConnection con = new OleDbConnection(connectionString))
        //        {
        //            var dataTable = new DataTable();
        //            string query = string.Format("SELECT * FROM [{0}]", sheetName);
        //            con.Open();
        //            OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
        //            adapter.Fill(dataTable);
        //            data.Tables.Add(dataTable);
        //        }
        //    }

        //    return data;
        //}

        //public static string[] GetExcelSheetNames(string connectionString)
        //{
        //    OleDbConnection con = null;
        //    DataTable dt = null;
        //    con = new OleDbConnection(connectionString);
        //    con.Open();
        //    dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        //    if (dt == null)
        //    {
        //        return null;
        //    }

        //    String[] excelSheetNames = new String[dt.Rows.Count];
        //    int i = 0;

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        excelSheetNames[i] = row["TABLE_NAME"].ToString();
        //        i++;
        //    }

        //    return excelSheetNames;
        //}


        public ExcelExtensionReponse Test(string fileName)
        {
            ExcelExtensionReponse excelExtensionReponse = new ExcelExtensionReponse();

            string JSONString = string.Empty;
            IExcelDataReader excelReader;
            try
            {
                FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);


                if(fileName.Contains("xlsx"))
                {
                    excelReader = ExcelReaderFactory.CreateReader(stream);
                }
                else
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }

                

                DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                excelReader.Close();
                bool getDesiredColumnExistence = result.Tables[0].Columns.Contains("package no");

                if (getDesiredColumnExistence)
                {



                    var regexItem = new Regex("[^0-9a-zA-Z]+");



                    for (int i = 0; i < result.Tables[0].Columns.Count; i++)
                    {

                        if (regexItem.IsMatch(result.Tables[0].Columns[i].ColumnName.ToString()))
                        {
                            result.Tables[0].Columns[i].ColumnName = "S_" + Regex.Replace(result.Tables[0].Columns[i].ColumnName, @"[^0-9a-zA-Z]+", "");
                        }
                    }

                    result.AcceptChanges();
                    JSONString = JsonConvert.SerializeObject(result.Tables[0]);
                    excelExtensionReponse.ExcelExtensionReponseData = JSONString;
                    excelExtensionReponse.Response = true;
                }
                else
                {
                    excelExtensionReponse.exception= new ArgumentException("Required Column 'package no' is not found");
                }
                
            }
            catch (Exception ex)
            {
                excelExtensionReponse.exception = ex;
            }

            return excelExtensionReponse;

        }

    }
}
