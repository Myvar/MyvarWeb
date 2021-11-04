using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Prototype.Http.Headers;
using Prototype.Utils;

namespace Prototype.Http
{
    public class HttpRequest : IDisposable
    {
        /// <summary>
        /// We use this so we can serialize the Request if need be
        /// </summary>
        [JsonProperty]
        private Guid BodyID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The headers for this req
        /// </summary>
        public List<HttpHeader> Headers { get; set; } = new List<HttpHeader>();

        /// <summary>
        /// The URL
        /// </summary>
        public string Url { get; set; }
        public string ForwardIp { get; set; }
        
        /// <summary>
        /// The Path of the req
        /// </summary>
        public string Path { get; set; }
        public ulong BodySize { get; set; }
        public Dictionary<string, string> Query { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Argument { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();

        public RequestMethod Method { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Stream Body
        {
            get { return MemoryStore.StreamStore[BodyID]; }
            set { MemoryStore.StreamStore[BodyID] = value; }
        }


        public void Dispose()
        {
            Body.Dispose();
        }

        public void ReadFromStream(NetworkStream stream)
        {
            //wait for data avalability
            while (!stream.DataAvailable) ;

            var sb = new StringBuilder();

            while (stream.DataAvailable)
            {
                var c = (char) (byte) stream.ReadByte();
                if (c == '\n')
                {
                    var x = sb.ToString();
                    if (string.IsNullOrEmpty(x.Trim()))
                    {
                        break;
                    }

                    var header = HttpHeader.Parse(x);
                    if (header != null)
                    {
                        Headers.Add(header);
                        if (header is CookieHeader ch)
                        {
                            foreach (var cookie in ch.Cookies)
                            {
                                Cookies.Add(cookie.Key, cookie.Value);
                            }
                        }

                        if (header is ContentLengthHeader cl)
                        {
                            BodySize = cl.Length;
                        }

                        if (header is ForwardedForHeader ff)
                        {
                            ForwardIp = ff.Ip;
                        }
                    }

                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }

            try
            {
                //@Hack this feels wrong so probably a @CleanUp to lol
                if (Headers.Count != 0)
                {
                    var meth = Headers.First(x => x is RequestMethodHeader) as RequestMethodHeader;

                    Url = meth?.Url;

                    Path = Url?.Split('?')[0];

                    if (Url != null && Url.Contains("?"))
                    {
                        var qry = Url?.Split('?')[1];
                        if (!string.IsNullOrEmpty(qry))
                        {
                            foreach (var s in qry.Split('&'))
                            {
                                var segs = s.Split('=');
                                Query.Add(segs[0], HttpUtility.UrlDecode(segs[1]));
                            }
                        }
                    }

                    Method = meth.Method;
                    //Logger.Debug(Method.ToString());
                }
            }
            catch (Exception e)
            {
                Logger.Debug(e.ToString());
            }

            if (Method == RequestMethod.Post)
            {
                Body = stream;
            }
        }

        public void ParseBodyIntoQuery()
        {
            var buf = new byte[BodySize];
            Body.Read(buf, 0, (int) BodySize);
            var qry = Encoding.ASCII.GetString(buf);
            if (!string.IsNullOrEmpty(qry))
            {
                foreach (var s in qry.Split('&'))
                {
                    var segs = s.Split('=');
                    Query.Add(segs[0], HttpUtility.UrlDecode(segs[1]));
                }
            }
        }

        public bool MatchPath(string path)
        {
            //Logger.Debug($"path: {path}, Path: {Path}");

            if (path == "*") return true;


            if (path.Contains("{")) //@Hack Write a proper lexer and parser you lazy basterd
            {
                var pathSegs = Path.Split('/');
                var inpathSegs = path.Split('/');

                if (pathSegs.Length != inpathSegs.Length) return false;

                for (int i = 0; i < inpathSegs.Length; i++)
                {
                    var a = pathSegs[i];
                    var b = inpathSegs[i];

                    if (b.StartsWith("{"))
                    {
                        if (!Argument.ContainsKey(b.TrimStart('{').TrimEnd('}')))
                        {
                            Argument.Add(b.TrimStart('{').TrimEnd('}'), a);
                            var aa = Argument.Last();
                            //Logger.Debug($"K:{aa.Key} V:{aa.Value}");
                        }
                    }
                    else if (a != b)
                    {
                        return false;
                    }
                }

                return true;
            }

            return Path.Trim().ToLower() == path.Trim().ToLower();
        }
    }
}