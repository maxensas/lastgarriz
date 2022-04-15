using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lastgarriz.Util
{
    // not in separated cs file
    internal enum Client : ushort
    {
        Default = 0x00
    }

    internal sealed class Net
    {
        private static Net instance = null;
        private static readonly object Instancelock = new();

        private static HttpClient Default { get; set; } = new();

        internal Net()
        {
            Default.Timeout = TimeSpan.FromSeconds(10);
            Default.DefaultRequestHeaders.Add("User-Agent", Strings.Net.UserAgent);
        }

        private static void Init()
        {
            if (instance == null)
            {
                lock (Instancelock)
                {
                    if (instance == null)
                    {
                        instance = new Net();
                    }
                }
            }
        }

        private static HttpClient GetClient(Client idClient)
        {
            return Default;
            /*
            return idClient switch
            {
                Client.Trade => Trade,
                Client.Update => Update,
                _ => Default,
            };
            */
        }

        internal static async Task<string> SendHTTP(string entity, string urlString, Client idClient)
        {
            Init();
            string result = string.Empty;
            HttpClient client = GetClient(idClient);

            try
            {
                HttpRequestMessage request = new();
                request.RequestUri = new Uri(urlString);
                request.Headers.ProxyAuthorization = null;
                //request.Headers.UserAgent.Add(new ProductInfoHeaderValue(Strings.Net.UserAgent));

                if (entity == null)
                {
                    request.Method = HttpMethod.Get;
                }
                else
                {
                    StringContent content = new(entity, Encoding.UTF8, "application/json");
                    //var content = new StringContent(entity);
                    //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    //content.Headers.ContentEncoding = new MediaTypeHeaderValue("utf-8");

                    request.Method = HttpMethod.Post;
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Content = content;
                }

                HttpResponseMessage response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode(); // if Http response failed : throw HttpRequestException

                if (response.Content is not null)
                {
                    result = await response.Content.ReadAsStringAsync();
                    //if error : {"error":{"code":3,"message":"Rate limit exceeded"}}
                    /*
                    if (response.Headers.Contains(Strings.Net.XrateLimitPolicy))
                    {
                    }
                    */
                }

            }
            catch (HttpRequestException)
            {
                /*
                var excep = new HttpRequestException("The request encountered an exception.", ex, ex.StatusCode);
                throw excep;
                */
                throw;
            }
            catch (TaskCanceledException ex)
            {
                if (ex.InnerException is TimeoutException)
                {
                    var excep = new TimeoutException("The request was canceled due to the configured timeout.", ex);
                    throw excep;
                }
                else
                {
                    var excep = new TaskCanceledException("A task was canceled.", ex);
                    throw excep;
                }
            }
            catch (Exception ex) // not done : ArgumentNullException / InvalidOperationException / AggregateException
            {
                if ((ex.InnerException is ThreadAbortException) || ex.Message.ToLowerInvariant().Contains("thread", StringComparison.Ordinal))
                {
                    var excep = new Exception("Abort called before the end, Application thread error", ex); // old way
                    throw excep;
                }
                else if (ex.InnerException is TimeoutException)
                {
                    var excep = new TimeoutException("The request was canceled due to the configured timeout (inner).", ex);
                    throw excep;
                }
                else if (ex.InnerException is TaskCanceledException)
                {
                    var excep = new TaskCanceledException("A task was canceled (inner).", ex);
                    throw excep;
                }
                else
                {
                    var exep = new Exception("Unidentified exception.", ex);
                    throw exep;
                }
            }
            return result;
        }

        /*
        internal void DisposeAll()
        {
            Default.Dispose();
        }
        */
    }
}
