using System;
using System.Collections.Generic;

namespace Bootpay.models
{
    public class ResToken : ResDefault
    {
        public new ResTokenData data { get; set; }
    }

    public class ResTokenData {

        public string token { get; set; }
        public long server_time { get; set; }
        public long expired_at { get; set; }
    }
}
