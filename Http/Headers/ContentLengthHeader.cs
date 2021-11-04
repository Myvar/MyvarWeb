using System.Linq;
using Prototype.Utils;

namespace Prototype.Http.Headers
{
    public class ContentLengthHeader : HttpHeader
    {
        public ulong Length { get; set; }

        public override bool TryParse(string line, out HttpHeader header)
        {
            line = line.Trim();

            //Logger.Debug(line);

            if (!line.ToLower().StartsWith("content-length:"))
            {
                header = null;
                return false;
            }

            var re = new ContentLengthHeader();

            re.Length = ulong.Parse(line.Split(":").Last().Trim());

            //Logger.Debug("Parsng content length");
            //Logger.Debug(line);
            //Logger.Debug(re.Length.ToString());
            //Logger.Debug(line.Split(":").Last().ToString());

            header = re;
            return true;
        }
    }
}