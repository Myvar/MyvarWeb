using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW.Core.Internals
{
    public class HttpResponce
    {
        public HttpHeader Header = new HttpHeader();
        public byte[] Body { get; set; }
        public bool ConnectionOpen = true;
        public bool FlushOnClose = true;

        public HttpResponce(string body)
        {
            Body = Utils.GetBytes(body);
            ConnectionOpen = false;
        }

        public HttpResponce()
        {

        }

        public byte[] Flush()
        {
            List<byte> Responce = new List<byte>();

            Responce.AddRange(Utils.GetBytes(Header.ToString()));
            Responce.AddRange(Body);

            return Responce.ToArray();
        }
    }
}
