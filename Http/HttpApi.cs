using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Prototype.Attributes;

namespace Prototype.Http
{
    public static class HttpApi
    {
        private static List<(HttpMethodAttribute, Func<HttpRequest, HttpResponse>)> _bindings { get; set; } =
            new List<(HttpMethodAttribute, Func<HttpRequest, HttpResponse>)>();

        static HttpApi()
        {
            foreach (var type in typeof(HttpApi).Assembly.GetTypes())
            {
                var apiAttr = type.GetCustomAttribute<HttpApiAttribute>();
                if (apiAttr != null)
                {
                    foreach (var method in type.GetMethods())
                    {
                        var methAttr = method.GetCustomAttribute<HttpMethodAttribute>();
                        if (methAttr != null)
                        {
                            _bindings.Add((methAttr, (x) => (HttpResponse) method.Invoke(null, new[] {(object) x})));
                        }
                    }
                }
            }
        }

        public static void Bind()
        {
            HttpServer.Handler = Handler;
        }

        private static HttpResponse Handler(HttpRequest req)
        {
            foreach (var (att, func) in _bindings)
            {
                if (att.Method == req.Method && req.MatchPath(att.Path))
                {
                    return func(req);
                }
            }

            if (req.Path == "/")
            {
                return new HttpResponse("Hello World");
            }

            return new HttpResponse("404");
        }
    }
}