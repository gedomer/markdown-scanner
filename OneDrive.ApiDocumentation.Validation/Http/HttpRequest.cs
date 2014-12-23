﻿namespace OneDrive.ApiDocumentation.Validation.Http
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    public class HttpRequest
    {
        public HttpRequest()
        {
            Headers = new WebHeaderCollection();
        }

        public string Method { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }

        public string Accept
        {
            get
            {
                return Headers["Accept"];
            }
            set
            {
                Headers["Accept"] = value;
            }
        }



        public WebHeaderCollection Headers { get; private set; }

        public HttpWebRequest PrepareHttpWebRequest(string baseUrl)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(baseUrl + Url);
            request.Method = Method;
            
            foreach (var key in Headers.AllKeys)
            {
                switch (key.ToLower())
                {
                    case "accept":
                        request.Accept = Headers[key];
                        break;
                    case "content-type":
                        request.ContentType = Headers[key];
                        break;
                    default:
                        request.Headers.Add(key, Headers[key]);
                        break;
                }
                
            }

            if (Body != null)
            {
                using (var stream = request.GetRequestStream())
                {
                    var writer = new StreamWriter(stream);
                    writer.Write(Body);
                }
            }

            return request;
        }

        public string FullHttpText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Method);
            sb.Append(" ");
            sb.Append(Url);
            sb.Append(" ");
            sb.AppendLine("HTTP/1.1");
            foreach (var header in Headers.AllKeys)
            {
                sb.AppendFormat("{0}: {1}", header, Headers[header]);
                sb.AppendLine();
            }
            sb.AppendLine();
            sb.Append(Body);
            
            return sb.ToString();
        }
    }
}