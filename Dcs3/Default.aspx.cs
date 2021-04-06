using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Dcs3
{
    public class Supplier
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Brutto { get; set; }
        public decimal Netto { get; set; }
    }

    public class SupplierGain
    {
        public string Name { get; set; }
        public decimal TotalGain { get; set; }
    }

    public partial class _Default : Page
    {
        public static string password { set; get; }
        public static string username { set; get; }
        public static string usernamePass { set; get; }
       

        public void Page_Load(object sender, EventArgs e)
        {

        }

        public void Button1_Click(object sender, EventArgs e)
        {
            WebRequest request = WebRequest.Create("http://web27.agency2000.co.il/Test/TestService.asmx/GetUsernameAndPassword ");
            
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = 0;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            
            string responseFromServer = reader.ReadToEnd();
           
            using (XmlReader xr = XmlReader.Create(new StringReader(responseFromServer)))
            {
                xr.ReadStartElement();
                usernamePass = xr.Value;
            }
              
            reader.Close();
            dataStream.Close();
            response.Close();

            Label1.Text = WebUtility.HtmlEncode(usernamePass);

            string[] multiArray = usernamePass.Split(new[] { "username: ", "password: ", "," }, StringSplitOptions.None);
            foreach (string str in multiArray)
            {
                if (str.Trim() != "")
                {
                    if (username != null)

                        password = str;
                    else username = str;

                }
            }

        }

        public void Button2_Click(object sender, EventArgs e)
        {
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://web27.agency2000.co.il/Test/TestService.asmx?op=SuppliersGain");
            request.Headers.Add("SOAP:Action");
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Method = "POST";

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            IAsyncResult asyncResult = request.BeginGetResponse(null, null);
            asyncResult.AsyncWaitHandle.WaitOne();

            string soapResult;
            SupplierGain[] supplierGains = new SupplierGain[3];
            supplierGains[0] = new SupplierGain();
            supplierGains[1] = new SupplierGain();
            supplierGains[2] = new SupplierGain();

            using (WebResponse webResponse = request.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
              
                int i = 0;
                int index = 0;
                i = soapResult.IndexOf("Name");
                while (i != -1)
                {
                    string a = "" ;
                    string temp = "";
                    while (a != "<")
                    {
                        i++;
                        
                        temp += a;
                        a = soapResult[i + 4].ToString();
                    }
                    supplierGains[index].Name = temp;
                    soapResult = soapResult.Substring(soapResult.IndexOf('N') + 15);
                    i = soapResult.IndexOf("TotalGain");
                    a = "";
                    temp = "";
                    while (a != "<")
                    {
                        i++;

                        temp += a;
                        a = soapResult[i + 9].ToString();
                    }

                    supplierGains[index].TotalGain = Convert.ToDecimal(temp);
                    soapResult = soapResult.Substring(soapResult.IndexOf('T') + 30);
                    i = soapResult.IndexOf("Name");
                    index++;
                }
         
            }
            Label2.Text = "Supplier 1 - Name:" + supplierGains[0].Name + ", " + "Total gain:" + supplierGains[0].TotalGain;
            Label3.Text = "Supplier 2 - Name:" + supplierGains[1].Name + ", " + "Total gain:" + supplierGains[1].TotalGain;
            Label4.Text = "Supplier 3 - Name:" + supplierGains[2].Name + ", " + "Total gain:" + supplierGains[2].TotalGain;


        }

        private static XmlDocument CreateSoapEnvelope()
        {

            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(
         @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
           xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
           xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
         <soap:Body>
    <SuppliersGain xmlns=""http://tempuri.org/"">
      <iUserName>"+username+@"</iUserName>
      <iPassword>"+password+@"</iPassword>
      <Suppliers>
        <Supplier>
          <Id>123</Id>
          <Name>Shiran</Name>
          <Brutto>6000</Brutto>
          <Netto>3000</Netto>
        </Supplier>
        <Supplier>
          <Id>456</Id>
          <Name>Rahamim</Name>
          <Brutto>8000</Brutto>
          <Netto>6000</Netto>
        </Supplier>
  <Supplier>
          <Id>789</Id>
          <Name>Chen</Name>
          <Brutto>10000</Brutto>
          <Netto>9000</Netto>
        </Supplier>
      </Suppliers>
    </SuppliersGain>
  </soap:Body>
</soap:Envelope>");
            return soapEnvelopeDocument;
        }
    }
}