using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Xml;
using UPS.Quincus.APP.Request;
using UPS.DataObjects.Shipment;
using HKShipmentServices;
using System.ServiceModel.Channels;
using System.IO;
using System.Net;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using UPS.Application.CustomLogs;

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

        public static OrderWebService GetServiceClient(string proxyUrl,string proxyUserName,string proxyPassword,string endPointInfo)
        {
            ChannelFactory<OrderWebService> myChannelFactory =
                new ChannelFactory<OrderWebService>(
                    GetBinding(proxyUrl),
                    new EndpointAddress(endPointInfo));//"http://osms.sit.sf-express.com:2080/osms/services/OrderWebService"));

            myChannelFactory.Credentials.UserName.UserName = proxyUserName;//System.Configuration.ConfigurationSettings.AppSettings["Username"].ToString();
            myChannelFactory.Credentials.UserName.Password = proxyPassword;// System.Configuration.ConfigurationSettings.AppSettings["Password"].ToString();

            return myChannelFactory.CreateChannel();
        }

        private static Binding GetBinding(string proxyUrl)
        {
            BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.UseDefaultWebProxy = false;

            basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            basicHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Basic;
            basicHttpBinding.UseDefaultWebProxy = false;
            //basicHttpBinding.ProxyAddress = new Uri(string.Format("http://{0}:{1}", proxyIpAddress, proxyPort));
            basicHttpBinding.ProxyAddress = new Uri(proxyUrl);

            return basicHttpBinding;

        }

        public static string CreateShipment(
            SFDataRequest uIOrderRequestBody,
            string checkWord, 
            string accessNumber, 
            string custID,
            bool proxyChannel,
            string proxyUrl,
            string proxyUserName,
            string proxyPassword,
            string endPoint)
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

                if (!proxyChannel)
                {
                    HKShipmentServices.OrderWebServiceClient wsClient = new HKShipmentServices.OrderWebServiceClient();
                    sfexpressService = wsClient.sfexpressServiceAsync(orderData, validateStr, accessNumber).Result;

                    Task.Run(() => AuditEventEntry.LogEntry(new DataObjects.LogData.LogDataModel()
                    {
                        dateTime = DateTime.Now,
                        apiTypes = DataObjects.LogData.APITypes.SF_Payload,
                        apiType = "SF_Payload",
                        LogInformation = new DataObjects.LogData.LogInformation()
                        {
                            LogResponse = sfexpressService.Body.Return,
                            LogRequest = data,
                            LogException = null
                        }
                    }));
                }

                if(proxyChannel)
                {

                    HttpWebRequest request = WebRequest.Create("http://osms.sit.sf-express.com:2080/osms/services/OrderWebService") as HttpWebRequest;
                    string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


  
                    string parameters = System.IO.File.ReadAllText(Path.Combine(executableLocation, "Request\\SOAPRequest.txt"));

                    parameters = string.Format(parameters, orderData, validateStr, accessNumber);

                    //"{\"name\":\"Umais\"}"; // or any other way to send parameters
                    WebProxy myProxy = new WebProxy(proxyUrl, false, null, new NetworkCredential(proxyUserName, proxyPassword));
                    request.Proxy = myProxy;
                    request.Method = "POST";
                    request.ContentLength = 0;
                    request.ContentType = "text/xml; charset=utf-8";
                    if (!string.IsNullOrEmpty(parameters))
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(parameters);
                        request.ContentLength = byteArray.Length;
                        Stream dataStream = request.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();
                    }
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    string responses = string.Empty;

                    using (Stream stream = response.GetResponseStream())
                    {
                        responses = (new StreamReader(stream)).ReadToEnd();
                    }

                    XmlDocument xmdcl = new XmlDocument();
                    xmdcl.LoadXml(responses);

                    Task.Run(() => AuditEventEntry.LogEntry(new DataObjects.LogData.LogDataModel()
                    {
                        dateTime = DateTime.Now,
                        apiTypes = DataObjects.LogData.APITypes.SF_Payload,
                        apiType = "SF_Payload",
                        LogInformation = new DataObjects.LogData.LogInformation()
                        {
                            LogResponse = data,
                            LogRequest = responses,
                            LogException = null
                        }
                    }));

                    return xmdcl.GetElementsByTagName("Return")[0].InnerText;
                }
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
