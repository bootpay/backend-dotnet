using System;
namespace Bootpay.models
{
    public class UserToken
    {
        public string user_id { get; set; }        
        public string email { get; set; }
        public string name { get; set; }
        public int gender { get; set; }
        public string birth { get; set; }
        public string phone { get; set; }
    }
}
