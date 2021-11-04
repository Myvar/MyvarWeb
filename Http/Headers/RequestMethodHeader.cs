using System;

namespace Prototype.Http.Headers
{
    public class RequestMethodHeader : HttpHeader
    {
        public RequestMethod Method { get; set; }
        public string Url { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }

        public override bool TryParse(string line, out HttpHeader header)
        {
            var segs = line.Split(' ');
            if (segs.Length != 3)
            {
                header = null;
                return false;
            }

            RequestMethod method;
            if (!Enum.TryParse(segs[0], true, out method))
            {
                header = null;
                return false;
            }

            var url = segs[1];

            if (!segs[2].ToLower().StartsWith("http/"))
            {
                header = null;
                return false;
            }

            var version = segs[2].Remove(0, 5);
            var verSegs = version.Split('.');

            if (verSegs.Length != 2)
            {
                header = null;
                return false;
            }


            int major, minor;
            if (!int.TryParse(verSegs[0], out major) || !int.TryParse(verSegs[1], out minor))
            {
                header = null;
                return false;
            }

            header = new RequestMethodHeader()
            {
                Major = major,
                Minor = minor,
                Method = method,
                Url = url
            };
            return true;
        }
    }
}