using System;
using Newtonsoft.Json;

namespace Bootpay.models
{
	public class BrowserOpenType
	{
		public string browser; //노출되는 판매자명 설정

		[JsonProperty("open_type")]
		public string openType; //노출되는 판매자명 설정
	}
}

