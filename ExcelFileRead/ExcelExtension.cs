using ExcelDataReader;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace ExcelFileRead
{
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

        public string Test(string fileName)
        {
            string JSONString = string.Empty;
            IExcelDataReader excelReader;
            try
            {
                FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);


                if(fileName.Contains("xlsx"))
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
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


                var regexItem = new Regex("[^0-9a-zA-Z]+");

                for (int i = 0; i < result.Tables[0].Columns.Count; i++)
                {
                    
                    if (regexItem.IsMatch(result.Tables[0].Columns[i].ColumnName.ToString()))
                    {
                        result.Tables[0].Columns[i].ColumnName = "S_" + Regex.Replace(result.Tables[0].Columns[i].ColumnName, @"[^0-9a-zA-Z]+", "");
                    }

                    
                }

                result.AcceptChanges();

                
                excelReader.Close();


                
                
                JSONString = JsonConvert.SerializeObject(result.Tables[0]);

               

              // var excelDataObject2 = JsonConvert.DeserializeObject<List<ExcelDataObject>>(JSONString);


                return JSONString;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

            //return JSONString;

        }

    }
}
