using System;
using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;

namespace Test
{
    /// <summary>
    /// Bootpay PG API 예제
    /// </summary>
    class Program
    {
        static BootpayApi bootpay = null!;

        static async Task Main(string[] args)
        {
            // === PG API 테스트 ===
            // Config에서 현재 환경에 맞는 키를 가져옴
            var mode = Config.CurrentEnv == "development" ? BootpayObject.MODE_DEVELOPMENT : "";
            bootpay = new BootpayApi(Config.PG.GetApplicationId(), Config.PG.GetPrivateKey(), mode);

            Console.WriteLine("Bootpay PG API Example");
            Console.WriteLine($"Environment: {Config.CurrentEnv}");
            Console.WriteLine("======================\n");

            await GoGetToken();
            // await GetReceipt();
            // await ReceiptCancel();
            // await GetBillingKey();
            // await RequestSubscribe();
            // await ReserveSubscribe();
            // await ReserveCancelSubscribe();
            // await DestroyBillingKey();
            // await GetUserToken();
            // await Confirm();
            // await Certificate();
            // await ShippingStart();
            // await GetBillingKeyTransfer();
            // await PublishBillingKeyTransfer();
            // await RequestAuthentication();
            // await ConfirmAuthentication();
            // await RealarmAuthentication();
            // await RequestCashReceipt();
            // await RequestCashReceiptCancel();
            // await RequestCashReceiptByBootpay();
            // await RequestCashReceiptCancelByBootpay();

            // === Commerce API 테스트 ===
            // await CommerceExample.Run();
        }

        static async Task GoGetToken()
        {
            try
            {
                var res = await bootpay.GetAccessToken();
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("goGetToken success: " + content);
                }
                else
                {
                    Console.WriteLine("goGetToken false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task GetReceipt()
        {
            try
            {
                var res = await bootpay.GetReceipt(Config.TestData.ReceiptId);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("getReceipt success: " + content);
                }
                else
                {
                    Console.WriteLine("getReceipt false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task Confirm()
        {
            try
            {
                var res = await bootpay.Confirm(Config.TestData.ReceiptIdConfirm);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("confirm success: " + content);
                }
                else
                {
                    Console.WriteLine("confirm false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task GetUserToken()
        {
            var userToken = new UserToken
            {
                userId = Config.TestData.UserId
            };
            try
            {
                var res = await bootpay.GetUserToken(userToken);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("getUserToken success: " + content);
                }
                else
                {
                    Console.WriteLine("getUserToken false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ReceiptCancel()
        {
            var cancel = new Cancel
            {
                receiptId = Config.TestData.ReceiptId,
                cancelUsername = "관리자",
                cancelMessage = "테스트 결제 취소"
            };

            try
            {
                var res = await bootpay.ReceiptCancel(cancel);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("receiptCancel success: " + content);
                }
                else
                {
                    Console.WriteLine("receiptCancel false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task GetBillingKey()
        {
            var subscribe = new Subscribe
            {
                orderName = "정기결제 테스트 아이템",
                subscriptionId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                pg = "나이스페이",
                cardNo = "5570**********1074", // 실제 테스트시에는 마스크처리가 아닌 숫자여야 함
                cardPw = "**",
                cardExpireYear = "**",
                cardExpireMonth = "**",
                cardIdentityNo = "" // 생년월일 또는 사업자 등록번호
            };

            try
            {
                var res = await bootpay.GetBillingKey(subscribe);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("getBillingKey success: " + content);
                }
                else
                {
                    Console.WriteLine("getBillingKey false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task GetBillingKeyTransfer()
        {
            try
            {
                var subscribe = new Subscribe
                {
                    orderName = "테스트 결제",
                    pg = "나이스페이",
                    bankName = "국민",
                    bankAccount = "67512341234472",
                    username = "홍길동",
                    identityNo = "901014",
                    phone = "01012341234",
                    subscriptionId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
                };

                var res = await bootpay.GetBillingKeyTransfer(subscribe);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("getBillingKeyTransfer success: " + content);
                }
                else
                {
                    Console.WriteLine("getBillingKeyTransfer false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task PublishBillingKeyTransfer()
        {
            try
            {
                var res = await bootpay.PublishBillingKeyTransfer(Config.TestData.ReceiptIdTransfer);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("publishBillingKeyTransfer success: " + content);
                }
                else
                {
                    Console.WriteLine("publishBillingKeyTransfer false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task RequestSubscribe()
        {
            var payload = new SubscribePayload
            {
                billingKey = Config.TestData.BillingKey,
                orderName = "아이템01",
                price = 1000,
                orderId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
            };

            try
            {
                var res = await bootpay.RequestSubscribe(payload);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("requestSubscribe success: " + content);
                }
                else
                {
                    Console.WriteLine("requestSubscribe false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ReserveSubscribe()
        {
            var reserveExecuteAt = DateTime.UtcNow.AddSeconds(10).ToString("yyyy-MM-ddTHH:mm:sszzz");

            var payload = new SubscribePayload
            {
                billingKey = Config.TestData.BillingKey,
                orderName = "아이템01",
                price = 1000,
                orderId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                reserveExecuteAt = reserveExecuteAt
            };

            Console.WriteLine("reserveSubscribe reserveExecuteAt: " + reserveExecuteAt);

            try
            {
                var res = await bootpay.ReserveSubscribe(payload);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("reserveSubscribe success: " + content);
                }
                else
                {
                    Console.WriteLine("reserveSubscribe false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ReserveSubscribeLookup()
        {
            try
            {
                var res = await bootpay.ReserveSubscribeLookup(Config.TestData.ReserveId);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("reserveSubscribeLookup success: " + content);
                }
                else
                {
                    Console.WriteLine("reserveSubscribeLookup false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ReserveCancelSubscribe()
        {
            try
            {
                var res = await bootpay.ReserveCancelSubscribe(Config.TestData.ReserveId);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("reserveCancelSubscribe success: " + content);
                }
                else
                {
                    Console.WriteLine("reserveCancelSubscribe false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task LookupBillingKey()
        {
            try
            {
                var res = await bootpay.LookupBillingKey(Config.TestData.ReceiptIdBilling);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("lookupBillingKey success: " + content);
                }
                else
                {
                    Console.WriteLine("lookupBillingKey false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task LookupBillingKeyByKey()
        {
            try
            {
                var res = await bootpay.LookupBillingKeyByKey(Config.TestData.BillingKey2);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("lookupBillingKeyByKey success: " + content);
                }
                else
                {
                    Console.WriteLine("lookupBillingKeyByKey false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task DestroyBillingKey()
        {
            try
            {
                var res = await bootpay.DestroyBillingKey(Config.TestData.BillingKey);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("destroyBillingKey success: " + content);
                }
                else
                {
                    Console.WriteLine("destroyBillingKey false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task RequestAuthentication()
        {
            var authentication = new Authentication
            {
                pg = "다날",
                method = "본인인증",
                username = "사용자명",
                identityNo = "0000000",
                carrier = "SKT",
                phone = "01010002000",
                siteUrl = "https://www.bootpay.co.kr",
                orderName = "회원 본인인증",
                authenticationId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
            };

            try
            {
                var res = await bootpay.RequestAuthentication(authentication);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("requestAuthentication success: " + content);
                }
                else
                {
                    Console.WriteLine("requestAuthentication false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ConfirmAuthentication()
        {
            try
            {
                var authParams = new AuthenticationParams
                {
                    receiptId = "636a0bc4d01c7e00331cd25e",
                    otp = "556659"
                };
                var res = await bootpay.ConfirmAuthentication(authParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("confirmAuthentication success: " + content);
                }
                else
                {
                    Console.WriteLine("confirmAuthentication false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task RealarmAuthentication()
        {
            try
            {
                var authParams = new AuthenticationParams
                {
                    receiptId = "6369dc33d01c7e00271cccad"
                };
                var res = await bootpay.RealarmAuthentication(authParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("realarmAuthentication success: " + content);
                }
                else
                {
                    Console.WriteLine("realarmAuthentication false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task Certificate()
        {
            try
            {
                var res = await bootpay.Certificate(Config.TestData.CertificateReceiptId);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("certificate success: " + content);
                }
                else
                {
                    Console.WriteLine("certificate false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ShippingStart()
        {
            var shipping = new Shipping
            {
                receiptId = Config.TestData.ReceiptIdEscrow,
                trackingNumber = "123456",
                deliveryCorp = "CJ대한통운"
            };
            shipping.user = new ShippingUser
            {
                username = "홍길동",
                phone = "01000000000",
                address = "서울특별시 종로구",
                zipcode = "08490"
            };

            try
            {
                var res = await bootpay.PutShippingStart(shipping);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("shippingStart success: " + content);
                }
                else
                {
                    Console.WriteLine("shippingStart false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task RequestCashReceipt()
        {
            var cashReceipt = new CashReceipt
            {
                pg = "토스",
                price = 1000,
                orderName = "테스트",
                cashReceiptType = "소득공제",
                identityNo = "01000000000",
                purchasedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                orderId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()
            };

            try
            {
                var res = await bootpay.RequestCashReceipt(cashReceipt);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("requestCashReceipt success: " + content);
                }
                else
                {
                    Console.WriteLine("requestCashReceipt false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task RequestCashReceiptCancel()
        {
            var cancel = new Cancel
            {
                receiptId = Config.TestData.ReceiptIdCash,
                cancelMessage = "테스트 결제",
                cancelUsername = "테스트 관리자"
            };

            try
            {
                var res = await bootpay.RequestCashReceiptCancel(cancel);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("requestCashReceiptCancel success: " + content);
                }
                else
                {
                    Console.WriteLine("requestCashReceiptCancel false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task RequestCashReceiptByBootpay()
        {
            var cashReceipt = new CashReceipt
            {
                receiptId = Config.TestData.ReceiptIdCash,
                username = "테스트",
                email = "test@bootpay.co.kr",
                phone = "01000000000",
                identityNo = "01000000000",
                cashReceiptType = "소득공제"
            };

            try
            {
                var res = await bootpay.RequestCashReceiptByBootpay(cashReceipt);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("requestCashReceiptByBootpay success: " + content);
                }
                else
                {
                    Console.WriteLine("requestCashReceiptByBootpay false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task RequestCashReceiptCancelByBootpay()
        {
            var cancel = new Cancel
            {
                receiptId = Config.TestData.ReceiptIdCash,
                cancelMessage = "테스트 결제",
                cancelUsername = "테스트 관리자"
            };

            try
            {
                var res = await bootpay.RequestCashReceiptCancelByBootpay(cancel);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("requestCashReceiptCancelByBootpay success: " + content);
                }
                else
                {
                    Console.WriteLine("requestCashReceiptCancelByBootpay false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
