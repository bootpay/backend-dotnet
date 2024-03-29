﻿using System;
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
    public class EasyUserTokenController : Controller
    {
        // 5. 사용자 토큰 발급 
        [HttpGet("easy_user_token")]
        public async Task<IActionResult> IndexAsync()
        {
            UserToken userToken = new UserToken();
            userToken.userId = "1234";
            userToken.email = "test@gmail.com";
            userToken.username = "홍길동";
            userToken.birth = "861014";
            userToken.gender = 0;


            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.GetUserToken(userToken);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

            return Ok(json);
        }
    }
}
