using System; 
using Newtonsoft.Json;

namespace Bootpay.models.response
{
    public class ResLink
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}
