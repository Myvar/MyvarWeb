using System;
using Prototype.Http;

namespace Prototype.Attributes
{
    public class HttpMethodAttribute : Attribute
    {
        public HttpMethodAttribute(RequestMethod method, string path)
        {
            Method = method;
            Path = path;
        }

        public RequestMethod Method { get; set; }
        public string Path { get; set; }

    }
}