using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace TinySpreadsheet
{
    public class Request
    {
        private String APIKey = @"4EKP78-75TGQYR3K7";
        private String WolframRequest;
        private String responseXML;

        public Request(String query)
        {
            query = query.Replace("+", "%2B"); //We have to do this because + is handled as an append in URLs.

            StringBuilder request = new StringBuilder();
            request.Append("http://api.wolframalpha.com/v2/query?input=");
            request.Append(query);
            request.Append("&appid=");
            request.Append(APIKey);

            WolframRequest = request.ToString();

            request.Clear();

        }

        public void Execute()
        {
            try
            {
                WebRequest request = WebRequest.Create(WolframRequest);
                WebResponse response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    Stream recieveStream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(recieveStream, Encoding.UTF8);
                    responseXML = readStream.ReadToEnd();
                }
            }
            catch (Exception)
            {
                responseXML = null;
            }
            
        }

        public String Result()
        {
            StringBuilder wolframResponse = new StringBuilder();
            using (XmlReader reader = XmlReader.Create((new StringReader(responseXML))))
            {
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(wolframResponse, ws))
                {
                   
                }

            }
            return wolframResponse.ToString();
        }
    }
}
