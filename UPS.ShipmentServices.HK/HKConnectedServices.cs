using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Xml;
using UPS.Quincus.APP.Request;
using UPS.DataObjects.Shipment;


namespace UPS.ShipmentServices.HK
{
    public static class HKConnectedServices
    {

        public static string ampReplacment(this String inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                if (inputString.Contains("&"))
                {
                    inputString = inputString.Replace("&", "&amp;");
                }
            }

            return inputString;
        }

        public static string CreateShipment(SFDataRequest uIOrderRequestBody,string checkWord, string accessNumber, string custID)
        {
            HKShipmentServices.sfexpressServiceResponse sfexpressService = null;
            
            string msg = "";
            try
            {
                string data = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                data += " <Request lang=\"" + "en" + "\"";
                data += " service =\"" + "apiOrderService" + "\"" + "><Head>" + accessNumber + "</Head>";
                data += " <Body><Order  reference_no1=\"" + uIOrderRequestBody.PKG_NR_TE + "\"";
                data += " j_contact =\"" + uIOrderRequestBody.SHP_CTC_TE.ampReplacment() + "\"";
                data += " j_tel=\"" + uIOrderRequestBody.SHP_PH_TE + "\"";
                data += " j_country=\"" + "HK" + "\"";
                data += " j_province=\"" + "HONGKONG" + "\"";
                data += " j_city=\"" + "Kwai Chung" + "\"";
                data += " j_county=\"" + "HK" + "\"";
                data += " j_address=\"" + uIOrderRequestBody.SHP_ADR_TE.ampReplacment() + "\"";
                data += " j_post_code=\"" + "852" + "\"";
                data += " d_email=\"" + uIOrderRequestBody.CSG_CTC_TE.ampReplacment() + "\"";
                data += " d_contact=\"" + uIOrderRequestBody.SHP_CPY_NA.ampReplacment() + "\"";
                data += " d_tel=\"" + uIOrderRequestBody.PH_NR + "\"";
                data += " d_country=\"" + "KR" + "\"";
                data += " d_address=\"" + uIOrderRequestBody.SHP_ADR_TR_TE.ampReplacment() + "\"";
                data += " d_post_code=\"" + uIOrderRequestBody.DST_PSL_TE + "\"";
                data += " custid=\"" + custID + "\"";
                data += " pay_method=\"" + "1" + "\"";
                data += " express_type =\"" + "101" + "\"";
                data += " parcel_quantity=\"" + "1" + "\"";
                data += " tax_pay_type=\"" + uIOrderRequestBody.BIL_TYP_TE + "\"";
                data += " currency=\"" + "USD" + "\"";
                data += " operate_type=\"" + "1" + "\"";

                if(uIOrderRequestBody.SMT_NR_TE.StartsWith("1Z"))
                {
                    data += " reference_no2=\"" + uIOrderRequestBody.SMT_NR_TE + "\"";
                }


                data += " order_cert_no=\"" + uIOrderRequestBody.IMP_NR + "\"" + ">";
                //Cargo
                //New Senarios for multiple cargos
                foreach (var cargo in uIOrderRequestBody.Cargos)
                {
                    data += "<Cargo name=\"" + cargo.FST_INV_LN_DES_TE + "\"";
                    data += " count =\"" + cargo.PCS_QTY_NR + "\"";
                    data += " unit =\"" + "PCS" + "\"";
                    data += " weight =\"" + cargo.PKG_WGT_DE + "\"";
                    data += " amount =\"" + cargo.SMT_VAL_DE + "\"";
                    data += " total_value =\"" + cargo.PCS_QTY_NR * cargo.SMT_VAL_DE + "\"";
                    data += " source_area =\"" + cargo.DIM_WGT_DE + "\"" + "/>";
                }

                //data += "<Cargo name=\"" + uIOrderRequestBody.fsT_INV_LN_DES_TE + "\"";
                //data += " count =\"" + uIOrderRequestBody.pcS_QTY_NR + "\"";
                //data += " unit =\"" + "PCS" + "\"";
                //data += " weight =\"" + uIOrderRequestBody.pkG_WGT_DE + "\"";
                //data += " amount =\"" + uIOrderRequestBody.smT_VAL_DE + "\"";
                //data += " total_value =\"" + uIOrderRequestBody.pcS_QTY_NR * uIOrderRequestBody.smT_VAL_DE + "\"";
                //data += " source_area =\"" + uIOrderRequestBody.diM_WGT_DE + "\"" + "/>";

                //Cargo
                data += " </Order></Body></Request>";

                XmlDocument xmldc = new XmlDocument();
                xmldc.LoadXml(data);

                
                //encryt xml with Base64
                string orderData = BAse64(data, true);
                //encrypt with Base64 and MD5 to get the validateStr
                string validateStr = BAse64(Md5(data + checkWord), true);

                

                HKShipmentServices.OrderWebServiceClient wsClient = new HKShipmentServices.OrderWebServiceClient();

                
               // wsClient.ClientCredentials.Windows.ClientCredential = 

                //string result = HKShipmentServices.sfexpressService(orderData, validateStr, client_id);

                sfexpressService = wsClient.sfexpressServiceAsync(orderData, validateStr, accessNumber).Result; 
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sfexpressService.Body.Return; 
        }

        


        public static string Md5(string strText)
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(strText));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }

            /// <summary>
            /// encryt with BAse64
            /// </summary>
            /// <param name="str">target string</param>
            /// <param name="mode">true-encrypt,false-decrypt</param>
            /// <returns></returns>
            public static string BAse64(string str, bool mode)
            {
                try
                {
                    if (mode)
                    {
                        return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(str));
                    }
                    else
                    {
                        return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(str));
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
    }
}
