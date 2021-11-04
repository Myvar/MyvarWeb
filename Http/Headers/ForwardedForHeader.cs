using System.Linq;
using Prototype.Utils;

namespace Prototype.Http.Headers
{
    public class ForwardedForHeader : HttpHeader
    {
        public string Ip { get; set; }

        public override bool TryParse(string line, out HttpHeader header)
        {
            line = line.Trim();

            //Logger.Debug(line);

            if (!line.ToLower().StartsWith("x-forwarded-for:"))
            {
                header = null;
                return false;
            }

            var re = new ForwardedForHeader();

            re.Ip = line.Split(":").Last().Trim();

            header = re;
            return true;
        }
    }
}