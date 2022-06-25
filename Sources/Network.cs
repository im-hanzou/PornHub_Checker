using System.Net;
using System.Net.Http.Headers;

namespace PHChecker
{
    public class Network
    {
        private CookieContainer cookieContainer { get; set; }

        public Network(CookieContainer? _cookieContainer = null)
        {
            if (_cookieContainer != null)
            {
                cookieContainer = _cookieContainer;
            }
            else
            {
                cookieContainer = new CookieContainer();
            }
        }

        public async Task<string?> POST_REQUEST(string URL, string PostData, bool Cookies, string? Referrer = null, string? Proxy = null, bool AutoRedirect = false)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.AllowAutoRedirect = AutoRedirect;
                
                if (Cookies)
                {
                    handler.UseCookies = Cookies;
                    handler.CookieContainer = cookieContainer;
                }
                HttpClient hClient = new HttpClient(handler);
                
                if (Proxy != null)
                {
                    string? proxy = ProxyCheck(Proxy);
                    if (proxy != null)
                    {
                        var socksHander = new HttpClientHandler
                        {
                            Proxy = new WebProxy(proxy)
                        };
                        hClient = new HttpClient(socksHander);
                    }
                    else { return null; }
                }
                var parameters = NormalizeData(PostData);

                hClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36");
                //hClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                hClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
                hClient.DefaultRequestHeaders.Referrer = new Uri(Referrer);
                var Answer = await hClient.PostAsync(new Uri(URL), /*new StringContent(PostData)*/new FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();

                if (Cookies)
                {
                    cookieContainer = handler.CookieContainer;
                }
                return Answer;
            }
        }

        public async Task<string?> GET_REQUEST(string URL, bool UseCookies = false, string? UserAgent = null, string? Proxy = null)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.AllowAutoRedirect = false;
                handler.UseCookies = UseCookies;

                if (Proxy != null)
                {
                    string? proxy = ProxyCheck(Proxy);
                    if (proxy != null)
                    {
                        handler.Proxy = new WebProxy(proxy);
                    }
                    else { return null; }
                }

                using (HttpClient hc = new HttpClient(handler))
                {
                    hc.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                    var result = await hc.GetAsync(new Uri(URL)).Result.Content.ReadAsStringAsync();
                    cookieContainer.Add(handler.CookieContainer.GetAllCookies());
                    return result;
                }
            }
        }

        public CookieCollection GetAllCookies()
        {
            return cookieContainer.GetAllCookies();
        }

        public bool AddCookie(Cookie cookie)
        {
            try
            {
                cookieContainer.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Normalizing your post data for web.
        /// </summary>
        /// <param name="Data">Post Data</param>
        /// <returns></returns>
        private Dictionary<string, string> NormalizeData(string Data)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            string[] Parametres = Data.Split('&');

            foreach (string Param in Parametres)
            {
                data.Add(Param.Split('=')[0], Param.Split('=')[1]);
            }
            return data;
        }

        public string? ProxyCheck(string proxy)
        {
            string[] types = { "socks5", "socks4", "https", "http" };
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    using (WebClient wc = new())
                    {
                        wc.Proxy = new WebProxy(String.Concat(types[i], "://", proxy));
                        if (wc.DownloadData("https://rt.pornhubpremium.com/").Length > 50)
                        {
                            return String.Concat(types[i], "://", proxy);
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            return null;
        }
    }
}
