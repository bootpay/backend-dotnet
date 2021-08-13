using System;
using System.Collections.Generic;

namespace Bootpay.models
{
    public class ResDefault
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}
