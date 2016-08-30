using MyvarWeb.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyvarWeb.Headers
{
    public class DateHeader : HttpField
    {
        public string Date { get; set; } = DateTime.Now.ToString();
      

        public override bool Id(string s)
        {
            throw new NotImplementedException();
        }

        public override HttpField Parse(string s)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Date: " + Date;
        }
    }
}
