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

        public bool success { get; set; }

        public Exception exception { get; set; }
    }

    public class ExcelExtension
    {


        

        public ExcelExtensionReponse Test(string fileName,string[] validationSet)
        {
            ExcelExtensionReponse excelExtensionReponse = new ExcelExtensionReponse();

            string JSONString = string.Empty;
            IExcelDataReader excelReader;
            try
            {
                FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);

                if (fileName.Contains("xlsx"))
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
                List<string> getValidationErrors;
                bool getDesiredColumnExistence = ColExistence(result, validationSet, out getValidationErrors);

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
                    excelExtensionReponse.success = true;
                }
                else
                {
                    excelExtensionReponse.exception = new ArgumentException("Required Columns "+ JsonConvert.SerializeObject(getValidationErrors) +" are not found");
                }

            }
            catch (Exception ex)
            {
                excelExtensionReponse.exception = ex;
            }

            return excelExtensionReponse;

        }

        private static bool ColExistence(DataSet result,string[] validationSet,out List<string> validationFailedSet)
        {
            bool desiredResult = true;
            int addressesCount = 0;
            validationFailedSet = new List<string>();

            for (int i = 0; i < result.Tables[0].Columns.Count; i++)
            {
                if(string.Equals(result.Tables[0].Columns[i].ToString(), "address", StringComparison.OrdinalIgnoreCase))
                {
                    addressesCount++;
                }
            }

            if (addressesCount == 2)
            {
                desiredResult = DefaultValidation(result, validationSet, validationFailedSet, desiredResult);
            }
            else
            {
                desiredResult = DefaultValidation(result, validationSet, validationFailedSet, desiredResult);
                validationFailedSet.Add("address");
                desiredResult = false;
            }
      
            return desiredResult;
        }

        private static bool DefaultValidation(DataSet result, string[] validationSet, List<string> validationFailedSet, bool desiredResult)
        {
            foreach (string validationColumn in validationSet)
            {
                if (!result.Tables[0].Columns.Contains(validationColumn))
                {
                    validationFailedSet.Add(validationColumn);
                    desiredResult = false;
                }
            }

            return desiredResult;
        }
    }
}
