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

        // Development 키
        const string DEV_APP_ID = "59bfc738e13f337dbd6ca48a";
        const string DEV_PRIVATE_KEY = "pDc0NwlkEX3aSaHTp/PPL/i8vn5E/CqRChgyEp/gHD0=";

        // Production 키
        const string PROD_APP_ID = "5b8f6a4d396fa665fdc2b5ea";
        const string PROD_PRIVATE_KEY = "rm6EYECr6aroQVG2ntW0A6LpWnkTgP4uQ3H18sDDUYw=";

        static async Task Main(string[] args)
        {
            // === PG API 테스트 ===
            // bootpay = new BootpayApi(DEV_APP_ID, DEV_PRIVATE_KEY, BootpayObject.MODE_DEVELOPMENT);
            // bootpay = new BootpayApi(PROD_APP_ID, PROD_PRIVATE_KEY);

            // Console.WriteLine("Bootpay PG API Example");
            // Console.WriteLine("======================\n");

            // await GoGetToken();
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
            await CommerceExample.Run();
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
            string receiptId = "62b12f4b6262500007629fec";
            try
            {
                var res = await bootpay.GetReceipt(receiptId);
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
            string receiptId = "62876963d01c7e00209b6028";
            try
            {
                var res = await bootpay.Confirm(receiptId);
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
                userId = "1234" // 개발사에서 관리하는 회원 고유 번호
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
                receiptId = "664ae6621a10a75af2b4b085",
                cancelUsername = "관리자",
                cancelMessage = "테스트 결제"
                // cancelPrice = 1000, // 부분취소 요청시
                // cancelId = "12342134" // 부분취소 요청시, 중복 부분취소 요청하는 실수를 방지하고자 할때 지정
            };
            // 가상계좌 환불 요청시
            // cancel.refund = new RefundData {
            //     bankAccount = "675601012341234",
            //     bankUsername = "홍길동",
            //     bankCode = BankCode.KB
            // };

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
                // receiptId: 692e8ded4631c7f34f76115b
                // billingKey: 692e8dee4631c7f34f76115e
                orderName = "정기결제 테스트 아이템",
                subscriptionId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                pg = "나이스페이",
                // cardNo = "5570**********1074", // 실제 테스트시에는 마스크처리가 아닌 숫자여야 함
                // cardPw = "**",
                // cardExpireYear = "**",
                // cardExpireMonth = "**",
                // cardIdentityNo = "" // 생년월일 또는 사업자 등록번호
                 cardNo = "5570420456641074", // 실제 테스트시에는 마스크처리가 아닌 숫자여야 함
                cardPw = "83",
                cardExpireYear = "26",
                cardExpireMonth = "12",
                cardIdentityNo = "861014" // 생년월일 또는 사업자 등록번호
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
                var res = await bootpay.PublishBillingKeyTransfer("66541bc4ca4517e69343e24c");
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
                billingKey = "692e8dee4631c7f34f76115e",
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
                billingKey = "692e8dee4631c7f34f76115e",
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
            string reserveId = "6490149ca575b40024f0b70d";
            try
            {
                var res = await bootpay.ReserveSubscribeLookup(reserveId);
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
            string reserveId = "692e701288acd62032ef1645";
            try
            {
                var res = await bootpay.ReserveCancelSubscribe(reserveId);
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
            string receiptId = "6317e646d01c7e0024170b47";
            try
            {
                var res = await bootpay.LookupBillingKey(receiptId);
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
            string billingKey = "66542dfb4d18d5fc7b43e1b6";
            try
            {
                var res = await bootpay.LookupBillingKeyByKey(billingKey);
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
            string billingKey = "628b2644d01c7e00209b6092";
            try
            {
                var res = await bootpay.DestroyBillingKey(billingKey);
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
                identityNo = "0000000", // 생년월일 + 주민번호 뒷 1자리
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
            string receiptId = "628ae7ffd01c7e001e9b6066";
            try
            {
                var res = await bootpay.Certificate(receiptId);
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
                receiptId = "628ae7ffd01c7e001e9b6066",
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
                receiptId = "62f48ae41fc192036f9f4b54",
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
                receiptId = "62e0f11f1fc192036b1b3c92",
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
                receiptId = "62e0f11f1fc192036b1b3c92",
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
