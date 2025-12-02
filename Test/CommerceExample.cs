using System;
using System.Threading.Tasks;
using Bootpay.Commerce;
using Bootpay.Commerce.Models;

namespace Test
{
    /// <summary>
    /// Bootpay Commerce API 예제
    /// </summary>
    class CommerceExample
    {
        static BootpayCommerceApi bootpay = null!;

        // Development 키
        const string DEV_CLIENT_KEY = "hxS-Up--5RvT6oU6QJE0JA";
        const string DEV_SECRET_KEY = "r5zxvDcQJiAP2PBQ0aJjSHQtblNmYFt6uFoEMhti_mg=";

        public static async Task Run()
        {
            bootpay = new BootpayCommerceApi(DEV_CLIENT_KEY, DEV_SECRET_KEY, BootpayCommerceObject.MODE_DEVELOPMENT);

            Console.WriteLine("Bootpay Commerce API Example");
            Console.WriteLine("============================\n");

            await GetToken();

            // === User ===
            // await UserJoin();
            // await UserToken();
            // await UserCheckExist();
            // await UserList();
            // await UserDetail();
            // await UserUpdate();
            // await UserDelete();

            // === UserGroup ===
            // await UserGroupCreate();
            // await UserGroupList();
            // await UserGroupDetail();
            // await UserGroupUpdate();
            // await UserGroupUserCreate();
            // await UserGroupUserDelete();

            // === Product ===
            // await ProductCreate();
            // await ProductList();
            // await ProductDetail();
            // await ProductUpdate();
            // await ProductStatus();
            // await ProductDelete();

            // === Order ===
            // await OrderList();
            // await OrderDetail();

            // === OrderSubscription ===
            await OrderSubscriptionList();
            // await OrderSubscriptionDetail();
            // await OrderSubscriptionPause();
            // await OrderSubscriptionResume();
            // await OrderSubscriptionTermination();
        }

        #region Token

        static async Task GetToken()
        {
            try
            {
                var res = await bootpay.GetAccessToken();
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("getToken success: " + content);
                }
                else
                {
                    Console.WriteLine("getToken false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region User

        static async Task UserJoin()
        {
            try
            {
                var user = new CommerceUser
                {
                    LoginId = "testuser01",
                    LoginPw = "password123",
                    Email = "test@bootpay.co.kr",
                    Phone = "01000000000",
                    Name = "홍길동"
                };

                var res = await bootpay.UserJoin(user);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userJoin success: " + content);
                }
                else
                {
                    Console.WriteLine("userJoin false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserToken()
        {
            try
            {
                string userId = "684fa4a6b0eacea5cd97464e";
                var res = await bootpay.UserToken(userId);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userToken success: " + content);
                }
                else
                {
                    Console.WriteLine("userToken false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserCheckExist()
        {
            try
            {
                var res = await bootpay.UserCheckExist("email-exist", "ehowlsla@bootpay.co.kr");
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userCheckExist success: " + content);
                }
                else
                {
                    Console.WriteLine("userCheckExist false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserList()
        {
            var listParams = new UserListParams
            {
                Page = 1,
                Limit = 10,
                Type = "user",
                Keyword = "홍길동"
            };

            try
            {
                var res = await bootpay.UserList(listParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userList success: " + content);
                }
                else
                {
                    Console.WriteLine("userList false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserDetail()
        {
            try
            {
                var res = await bootpay.UserDetail("684fa4a6b0eacea5cd97464e");
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userDetail success: " + content);
                }
                else
                {
                    Console.WriteLine("userDetail false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserUpdate()
        {
            var user = new CommerceUser
            {
                UserId = "684fa4a6b0eacea5cd97464e",
                Phone = "01012345678",
                Name = "홍길동수정"
            };

            try
            {
                var res = await bootpay.UserUpdate(user);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userUpdate success: " + content);
                }
                else
                {
                    Console.WriteLine("userUpdate false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserDelete()
        {
            try
            {
                var res = await bootpay.UserDelete("684fa4a6b0eacea5cd97464e");
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userDelete success: " + content);
                }
                else
                {
                    Console.WriteLine("userDelete false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region UserGroup

        static async Task UserGroupCreate()
        {
            try
            {
                var userGroup = new CommerceUserGroup
                {
                    CompanyName = "테스트 그룹",
                    CorporateType = CorporateType.Individual
                };

                var res = await bootpay.UserGroupCreate(userGroup);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userGroupCreate success: " + content);
                }
                else
                {
                    Console.WriteLine("userGroupCreate false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserGroupList()
        {
            var listParams = new UserGroupListParams
            {
                Page = 1,
                Limit = 10
            };

            try
            {
                var res = await bootpay.UserGroupList(listParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userGroupList success: " + content);
                }
                else
                {
                    Console.WriteLine("userGroupList false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserGroupDetail()
        {
            try
            {
                var res = await bootpay.UserGroupDetail("684a76feb0eacea5cd974603");
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userGroupDetail success: " + content);
                }
                else
                {
                    Console.WriteLine("userGroupDetail false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserGroupUpdate()
        {
            var userGroup = new CommerceUserGroup
            {
                UserGroupId = "684a76feb0eacea5cd974603",
                CompanyName = "테스트 그룹 수정"
            };

            try
            {
                var res = await bootpay.UserGroupUpdate(userGroup);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userGroupUpdate success: " + content);
                }
                else
                {
                    Console.WriteLine("userGroupUpdate false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserGroupUserCreate()
        {
            try
            {
                var res = await bootpay.UserGroupUserCreate("684a76feb0eacea5cd974603", "684fa4a6b0eacea5cd97464e");
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userGroupUserCreate success: " + content);
                }
                else
                {
                    Console.WriteLine("userGroupUserCreate false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task UserGroupUserDelete()
        {
            try
            {
                var res = await bootpay.UserGroupUserDelete("684a76feb0eacea5cd974603", "684fa4a6b0eacea5cd97464e");
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("userGroupUserDelete success: " + content);
                }
                else
                {
                    Console.WriteLine("userGroupUserDelete false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Product

        static async Task ProductCreate()
        {
            try
            {
                var product = new CommerceProduct
                {
                    Name = "테스트 상품",
                    DisplayPrice = 10000,
                    Type = 1
                };

                var res = await bootpay.ProductCreate(product);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("productCreate success: " + content);
                }
                else
                {
                    Console.WriteLine("productCreate false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ProductList()
        {
            var listParams = new ProductListParams
            {
                Page = 1,
                Limit = 10
            };

            try
            {
                var res = await bootpay.ProductList(listParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("productList success: " + content);
                }
                else
                {
                    Console.WriteLine("productList false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ProductDetail()
        {
            try
            {
                var res = await bootpay.ProductDetail("product_id_here");
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("productDetail success: " + content);
                }
                else
                {
                    Console.WriteLine("productDetail false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ProductUpdate()
        {
            var product = new CommerceProduct
            {
                ProductId = "product_id_here",
                Name = "테스트 상품 수정",
                DisplayPrice = 20000
            };

            try
            {
                var res = await bootpay.ProductUpdate(product);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("productUpdate success: " + content);
                }
                else
                {
                    Console.WriteLine("productUpdate false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ProductStatus()
        {
            var statusParams = new ProductStatusParams
            {
                ProductId = "product_id_here",
                Status = 1
            };

            try
            {
                var res = await bootpay.ProductStatus(statusParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("productStatus success: " + content);
                }
                else
                {
                    Console.WriteLine("productStatus false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ProductDelete()
        {
            try
            {
                var res = await bootpay.ProductDelete("product_id_here");
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("productDelete success: " + content);
                }
                else
                {
                    Console.WriteLine("productDelete false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Order

        static async Task OrderList()
        {
            var listParams = new OrderListParams
            {
                Page = 1,
                Limit = 10
            };

            try
            {
                var res = await bootpay.OrderList(listParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("orderList success: " + content);
                }
                else
                {
                    Console.WriteLine("orderList false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task OrderDetail()
        {
            try
            {
                var res = await bootpay.OrderDetail("order_id_here");
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("orderDetail success: " + content);
                }
                else
                {
                    Console.WriteLine("orderDetail false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region OrderSubscription

        static async Task OrderSubscriptionList()
        {
            var listParams = new OrderSubscriptionListParams
            {
                Page = 1,
                Limit = 10
            };

            try
            {
                var res = await bootpay.OrderSubscriptionList(listParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("orderSubscriptionList success: " + content);
                }
                else
                {
                    Console.WriteLine("orderSubscriptionList false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task OrderSubscriptionDetail()
        {
            try
            {
                var res = await bootpay.OrderSubscriptionDetail("order_subscription_id_here");
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("orderSubscriptionDetail success: " + content);
                }
                else
                {
                    Console.WriteLine("orderSubscriptionDetail false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task OrderSubscriptionPause()
        {
            var pauseParams = new OrderSubscriptionPauseParams
            {
                OrderSubscriptionId = "order_subscription_id_here"
            };

            try
            {
                var res = await bootpay.OrderSubscriptionPause(pauseParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("orderSubscriptionPause success: " + content);
                }
                else
                {
                    Console.WriteLine("orderSubscriptionPause false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task OrderSubscriptionResume()
        {
            var resumeParams = new OrderSubscriptionResumeParams
            {
                OrderSubscriptionId = "order_subscription_id_here"
            };

            try
            {
                var res = await bootpay.OrderSubscriptionResume(resumeParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("orderSubscriptionResume success: " + content);
                }
                else
                {
                    Console.WriteLine("orderSubscriptionResume false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task OrderSubscriptionTermination()
        {
            var terminationParams = new OrderSubscriptionTerminationParams
            {
                OrderSubscriptionId = "order_subscription_id_here"
            };

            try
            {
                var res = await bootpay.OrderSubscriptionTermination(terminationParams);
                var content = await res.Content.ReadAsStringAsync();
                if (res.IsSuccessStatusCode)
                {
                    Console.WriteLine("orderSubscriptionTermination success: " + content);
                }
                else
                {
                    Console.WriteLine("orderSubscriptionTermination false: " + content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion
    }
}
