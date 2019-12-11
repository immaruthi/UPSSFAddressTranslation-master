using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Xml;
using UPS.Quincus.APP.Request;


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

        public static string CreateShipment(UIOrderRequestBodyData uIOrderRequestBody,string checkWord, string accessNumber)
        {
            HKShipmentServices.sfexpressServiceResponse sfexpressService = null;
            
            string msg = "";
            try
            {
                string data = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                data += " <Request lang=\"" + "en" + "\"";
                data += " service =\"" + "apiOrderService" + "\"" + "><Head>" + accessNumber + "</Head>";
                data += " <Body><Order  reference_no1=\"" + uIOrderRequestBody.pkG_NR_TE + "\"";
                data += " j_contact =\""+ uIOrderRequestBody.shP_CTC_TE.ampReplacment() + "\"";
                data += " j_tel=\"" + uIOrderRequestBody.shP_PH_TE + "\"";
                data += " j_country=\"" + "HK" + "\"";
                data += " j_province=\"" + "HONGKONG" + "\"";
                data += " j_city=\"" + "Kwai Chung" + "\"";
                data += " j_county=\"" + "HK" + "\"";
                data += " j_address=\"" + uIOrderRequestBody.shP_ADR_TE.ampReplacment() + "\"";
                data += " j_post_code=\"" + "852" + "\"";
                data += " d_email=\"" + uIOrderRequestBody.csG_CTC_TE.ampReplacment() + "\"";
                data += " d_contact=\"" + uIOrderRequestBody.shP_CPY_NA + "\"";
                data += " d_tel=\"" + uIOrderRequestBody.pH_NR + "\"";
                data += " d_country=\"" + "KR" + "\"";
                data += " d_address=\"" + uIOrderRequestBody.shP_ADR_TR_TE + "\"";
                data += " d_post_code=\"" + uIOrderRequestBody.dsT_PSL_TE + "\"";
                data += " custid=\"" + "8526898978" + "\"";
                data += " pay_method=\"" + "1" + "\"";
                data += " express_type =\"" + "1" + "\"";
                data += " parcel_quantity=\"" + "1" + "\"";
                data += " tax_pay_type=\"" + "1" + "\"";
                data += " currency=\"" + "USD" + "\"";
                data += " operate_type=\"" + "1" + "\"";
                data += " order_cert_no=\"" + uIOrderRequestBody.imP_NR + "\"" + ">";
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
