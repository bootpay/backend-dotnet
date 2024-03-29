﻿using System;
using System.Collections.Generic; 

namespace Bootpay.models
{
    public class BankCode
    {
        public static readonly Dictionary<string, string> bankMap = new Dictionary<string, string>
        {
            { "한국은행", "001" },
            { "기업은행", "003" },
            { "외환은행", "005" },
            { "수협은행", "007" },
            { "농협은행", "011" },
            { "우리은행", "020" },
            { "SC은행", "023" },
            { "대구은행", "031" },
            { "부산은행", "032" },
            { "광주은행", "034" },
            { "경남은행", "039" },
            { "우체국", "071" },
            { "KEB하나은행", "081" },
            { "신한은행", "088" },
            { "케이뱅크", "089" },
            { "카카오뱅크", "090" },
        };

        public static string getCode(string name) {
            if (name == null) return ""; 
            return bankMap[name];
        }
    }
}
