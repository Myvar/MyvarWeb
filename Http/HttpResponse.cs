using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Prototype.Utils;

namespace Prototype.Http
{
    /// <summary>
    /// Http Response is used to send content to the user
    /// </summary>
    public class HttpResponse : IDisposable
    {
        /// <summary>
        /// We use this so we can serialize the Response if need be
        /// </summary>
        [JsonProperty]
        private Guid BodyID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// We use this so we can serialize the Response if need be
        /// </summary>
        [JsonIgnore]
        public Stream Content
        {
            get => MemoryStore.StreamStore[BodyID];
            set => MemoryStore.StreamStore[BodyID] = value;
        }


        public string ContentType { get; set; } = "text/html";

        public Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();
        public List<string> CustomHeaders { get; set; } = new List<string>();

        public HttpResponse()
        {
        }

        /// <summary>
        /// return a plain string
        /// </summary>
        /// <param name="res"></param>
        public HttpResponse(string res)
        {
            Content = new MemoryStream(Encoding.ASCII.GetBytes(res));
        }

        /// <summary>
        /// return a string with a mime type
        /// </summary>
        /// <param name="res">The string</param>
        /// <param name="mime">the mime type</param>
        public HttpResponse(string res, string mime)
        {
            Content = new MemoryStream(Encoding.ASCII.GetBytes(res));
            ContentType = mime;
        }

        /// <summary>
        /// Return a Json type
        /// </summary>
        /// <param name="json">the object to serialize</param>
        public HttpResponse(object json)
        {
            Content = new MemoryStream(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(json)));
            ContentType = "application/json";
        }

        /// <summary>
        /// Return the content of a stream (good way to send a file)
        /// </summary>
        /// <param name="stream">The Stream</param>
        public HttpResponse(Stream stream)
        {
            Content = stream;
        }

        /// <summary>
        /// Return the content of a stream (good way to send a file) with mime type
        /// </summary>
        /// <param name="stream">The Stream</param>
        /// <param name="mime">mime type</param>
        public HttpResponse(Stream stream, string mime)
        {
            Content = stream;
            ContentType = mime;
        }

        /// <summary>
        /// Return raw bytes
        /// </summary>
        /// <param name="res">The body</param>
        public HttpResponse(byte[] res)
        {
            if (res == null)
            {
                Content = new MemoryStream();
            }
            else
            {
                Content = new MemoryStream(res);
            }
        }

        public HttpResponse(byte[] res, string mimeType)
        {
            if (res == null)
            {
                Content = new MemoryStream();
            }
            else
            {
                Content = new MemoryStream(res);
            }

            ContentType = mimeType;
        }


        /*public HttpResponse(object content, string source)
        {
            var body = Resource.GetResourceFile(source);

            var template = Handlebars.Compile(body);

            Content = Content = new MemoryStream(Encoding.ASCII.GetBytes(template(content)));

            Logger.Debug("Transplanting properties");
            foreach (var property in content.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(HttpResponse))
                {
                    Logger.Debug("Found Transplant Prop");
                    var resp = (HttpResponse) property.GetValue(content);
                    CustomHeaders.AddRange(resp.CustomHeaders);
                    Logger.Debug("Copied Custom Headers");
                    foreach (var keyValuePair in resp.Cookies)
                    {
                        Cookies.Add(keyValuePair.Key, keyValuePair.Value);
                    }

                    Logger.Debug("Copied Cookies");
                }
            }
        }*/


        public void Flush(StreamWriter sw)
        {
            //Logger.Debug("Writing Response");

            sw.WriteLine("HTTP/1.1 200 OK");
            //if (Content.Length != 0) sw.WriteLine("Content-Length: " + Content.Length);
            sw.WriteLine("Content-Type: " + ContentType);
            sw.WriteLine("Date: " + DateTime.Now.ToUniversalTime().ToString("r"));
            sw.WriteLine("Server: mws");
            sw.WriteLine("Connection: keep-alive");
            foreach (var header in CustomHeaders)
            {
                sw.WriteLine(header);
            }

            foreach (var cookie in Cookies)
            {
                sw.WriteLine(
                    $"Set-Cookie: {cookie.Key}={cookie.Value}; Path=/; Expires=" +
                    DateTime.Now.AddDays(14).ToUniversalTime().ToString("r"));
            }

            sw.WriteLine("");
            sw.Flush();
            /*  if (Content != nu)  <-- @Hack */
            Content.CopyTo(sw.BaseStream);
            sw.BaseStream.Flush();
            sw.Flush();
        }

        public void Dispose()
        {
            Content.Dispose();
        }
    }
}