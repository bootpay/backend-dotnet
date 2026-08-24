using System.Net;
using System.Net.Sockets;
using System.Text;
using Bootpay.Commerce;

namespace Bootpay.Tests.Wire
{
    /// <summary>
    /// Wire-format 검증용 로컬 mock HTTP 서버.
    /// SDK 가 실제로 보낸 method / URL / 헤더 / 바디를 기록하고 {"success":true} 를 응답한다.
    /// 외부 네트워크 없이 요청 규약(경로·인자·헤더)을 단정할 수 있다.
    /// </summary>
    public sealed class MockServer : IDisposable
    {
        private readonly HttpListener _listener;
        private readonly CancellationTokenSource _cts = new();
        public string BaseUrl { get; }
        public List<RecordedRequest> Requests { get; } = new();
        public string ResponseBody { get; set; } = "{\"success\":true,\"data\":{}}";

        public MockServer()
        {
            var port = FreePort();
            BaseUrl = $"http://127.0.0.1:{port}/";
            _listener = new HttpListener();
            _listener.Prefixes.Add(BaseUrl);
            _listener.Start();
            _ = Task.Run(ListenLoop);
        }

        public RecordedRequest LastRequest => Requests[Requests.Count - 1];

        private static int FreePort()
        {
            var probe = new TcpListener(IPAddress.Loopback, 0);
            probe.Start();
            var port = ((IPEndPoint)probe.LocalEndpoint).Port;
            probe.Stop();
            return port;
        }

        private async Task ListenLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                HttpListenerContext context;
                try
                {
                    context = await _listener.GetContextAsync();
                }
                catch (Exception)
                {
                    return; // listener stopped
                }

                var request = context.Request;
                string body;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    body = await reader.ReadToEndAsync();
                }

                var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var key in request.Headers.AllKeys)
                {
                    if (key != null) headers[key] = request.Headers[key] ?? "";
                }

                lock (Requests)
                {
                    Requests.Add(new RecordedRequest
                    {
                        Method = request.HttpMethod,
                        PathAndQuery = request.Url?.PathAndQuery ?? "",
                        Headers = headers,
                        Body = body,
                        ContentType = request.ContentType
                    });
                }

                var payload = Encoding.UTF8.GetBytes(ResponseBody);
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/json";
                context.Response.ContentLength64 = payload.Length;
                await context.Response.OutputStream.WriteAsync(payload);
                context.Response.Close();
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            try { _listener.Stop(); } catch (Exception) { /* already stopped */ }
        }
    }

    public sealed class RecordedRequest
    {
        public string Method { get; set; } = "";
        public string PathAndQuery { get; set; } = "";
        public Dictionary<string, string> Headers { get; set; } = new();
        public string Body { get; set; } = "";
        public string? ContentType { get; set; }
    }

    /// <summary>
    /// _baseUrl 을 mock 서버로 돌린 Commerce API — wire-format 테스트 전용.
    /// </summary>
    public sealed class WireCommerceApi : BootpayCommerceApi
    {
        public WireCommerceApi(string baseUrl) : base("test_client_key", "test_secret_key")
        {
            _baseUrl = baseUrl;
        }
    }

    /// <summary>
    /// _baseUrl 을 mock 서버로 돌린 PG API — wire-format 테스트 전용.
    /// </summary>
    public sealed class WireBootpayApi : BootpayApi
    {
        public WireBootpayApi(string baseUrl) : base("test_application_id", "test_private_key")
        {
            _baseUrl = baseUrl;
        }
    }

    /// <summary>
    /// client_key/secret_key PG 요청의 공통 인증 헤더를 검증하는 wire 테스트 전용 객체.
    /// </summary>
    public sealed class WireClientKeyBootpayObject : BootpayObject
    {
        public WireClientKeyBootpayObject(string baseUrl) : base(clientKey: "test_client_key", secretKey: "test_secret_key")
        {
            _baseUrl = baseUrl;
        }
    }
}
