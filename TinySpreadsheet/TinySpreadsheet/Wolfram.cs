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
        private const String ApiKey = @"4EKP78-75TGQYR3K7";
        private readonly String wolframRequest;
        private String responseXml;

        /// <summary>
        /// Builds a WolframAlpha webrequest from a cell.
        /// </summary>
        /// <param name="query"></param>
        public Request(String query)
        {
            query = query.Replace("+", "%2B"); //We have to do this because + is handled as an append in URLs.

            StringBuilder request = new StringBuilder();
            request.Append("http://api.wolframalpha.com/v2/query?input=");
            request.Append(query);
            request.Append("&appid=");
            request.Append(ApiKey);

            wolframRequest = request.ToString();

            request.Clear();

        }

        /// <summary>
        /// Executes a web request
        /// </summary>
        public void Execute()
        {
            try
            {
                WebRequest request = WebRequest.Create(wolframRequest);
                WebResponse response = request.GetResponse();

                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    Stream recieveStream = response.GetResponseStream();
                    if (recieveStream != null)
                    {
                        StreamReader readStream = new StreamReader(recieveStream, Encoding.UTF8);
                        responseXml = readStream.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                responseXml = null;
            }
            
        }

        /// <summary>
        /// Parses the response XML to retreive the proper answer.
        /// </summary>
        /// <returns></returns>
        public String Result()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(responseXml));
            if (doc.DocumentElement != null)
            {
                XmlNodeList nodes = doc.DocumentElement.GetElementsByTagName("pod");
                return nodes[1].InnerText;
            }

            return null;
        }
    }
}
