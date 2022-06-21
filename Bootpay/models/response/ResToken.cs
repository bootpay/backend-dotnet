using System;
using System.Collections.Generic;

namespace Bootpay.models
{
    public class ResToken
    {

        public string access_token { get; set; }
        public long server_time { get; set; }
        public long expired_at { get; set; }
        //public new ResTokenData data { get; set; }
    }

    //public class ResTokenData {

    //    public string access_token { get; set; }
    //    public long server_time { get; set; }
    //    public long expired_at { get; set; }
    //}
}
