using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sample.Models;


namespace Sample.Controllers
{
    public class TokenController : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            BootpayApi api = new BootpayApi("5b8f6a4d396fa665fdc2b5ea", "rm6EYECr6aroQVG2ntW0A6LpWnkTgP4uQ3H18sDDUYw=");           
            var res = await api.GetAccessToken();            
            var jsonString = JsonConvert.SerializeObject(res); 

            return Ok(jsonString);
        } 
    }
}
