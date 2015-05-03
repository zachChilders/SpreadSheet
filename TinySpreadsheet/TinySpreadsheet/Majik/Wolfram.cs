using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace TinySpreadsheet.Majik
{
    public class Request
    {
        private const String ApiKey = @"4EKP78-75TGQYR3K7";
        private readonly String wolframRequest;
        private String responseXml;
        private static Dictionary<String, long> wordToNumber;

        /// <summary>
        /// Static Constructor intitializes the dictionary so each new request doesn't have to.
        /// </summary>
        static Request()
        {
            wordToNumber = new Dictionary<string, long>();
            wordToNumber["trillion"] = 1000000000000;
            wordToNumber["billion"] = 1000000000;
            wordToNumber["million"] = 1000000;
            wordToNumber["thousand"] = 1000;
        }

        /// <summary>
        /// Builds a WolframAlpha webrequest from a cell.
        /// </summary>
        /// <param name="query">A string to be sent to Wolfram.</param>
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
        private void Execute()
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
        /// <returns>The result from Wolfram.</returns>
        private String Result()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(responseXml));
            if (doc.DocumentElement != null)
            {
                XmlNodeList nodes = doc.DocumentElement.GetElementsByTagName("pod");
                return ParseWolfram(nodes[1].InnerText);
            }

            return "NaN";
        }

        /// <summary>
        /// This method is used to ensure proper numbering is returned in wolfram responses.
        /// </summary>
        /// <param name="request">A string from Wolfram to be parsed.</param>
        /// <returns>A number representation of Wolfram's result.</returns>
        private String ParseWolfram(String request)
        {
            Double tmp = 1.0f;
            //If we just get a single number back, that's a good thing, so just return that.
            if (!Double.TryParse(request, out tmp))
            {
                //Split the request into lines
                String [] lines = request.Split('\n');
                //We probably only care about the first line
                String[] words = lines[0].Split(' ');
                List<Double> bucket = new List<double>();
                
                foreach (string t in words)
                {
                    //Check each word if it's a number.
                    if (!Double.TryParse(t, out tmp))
                    {
                        //If it's not a number, but can be converted to one, convert it and then use it
                        if (wordToNumber.ContainsKey(t))
                        {
                            bucket.Add(wordToNumber[t]);
                        }
                    }
                    else //These words were successfully parsed as doubles.
                    {
                        bucket.Add(tmp);
                    }
                }
                //Multiply everything together to get one number.
                tmp = bucket.Aggregate<double, double>(1.0f, (current, num) => current*num);
            }

            return tmp.ToString();
        }

        /// <summary>
        /// Public method to execute the wolfram query and return the result.
        /// </summary>
        /// <returns>The result from Wolfram.</returns>
        public String Run()
        {
            Execute();
            return Result();

        }
    }
}
