using System.Linq;
using Prototype.Utils;

namespace Prototype.Http.Headers
{
    public class HostHeader : HttpHeader
    {
        public string Host { get; set; }

        public override bool TryParse(string line, out HttpHeader header)
        {
            line = line.Trim();

            //Logger.Debug(line);

            if (!line.ToLower().StartsWith("host:"))
            {
                header = null;
                return false;
            }

            var re = new HostHeader();

            re.Host = line.Split(":").Last().Trim();

            header = re;
            return true;
        }
    }
}